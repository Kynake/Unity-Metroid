using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusController : LivingEntity {
  private struct BoxColliderVectors {
    public Vector2 offset;
    public Vector2 size;
  }

  public float jumpHeight;

  public float toMorphballHop; // in tiles
  public float fromMorphballHop; // in tiles

  public float recoilStrength;
  public float maxRecoilHorizontalAngle;

  public LayerMask enemyDamageLayer;

  private bool _canLongJumpAgain = true;
  // private bool _isLongJumping = false;
  // private bool _isShortJumping = false;
  private bool _jumpInertiaStopped = false;
  private bool _canExitMorphball = false;
  private BoxColliderVectors _tallBox;
  private BoxColliderVectors _shortBox;

  // Object components
  private SamusState _samusState;
  private SamusInput _samusInput;
  private SamusAudio _samusAudio;

  // Holding vars
  private List<ContactPoint2D> _holdingContactPoints = new List<ContactPoint2D>();

  private new void Awake() {
    base.Awake();

    // Scripts
    _samusState = GetComponent<SamusState>();
    if (_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }

    _samusInput = GetComponent<SamusInput>();
    if (_samusInput == null) {
      Debug.LogError("SamusInput Script not found!");
      return;
    }

    _samusAudio = GetComponent<SamusAudio>();
    if (_samusAudio == null) {
      Debug.LogError("SamusAudio Script not found!");
      return;
    }
  }

  private void Start() {

    // Initialize structs
    _tallBox.offset = _boxCollider.offset;
    _tallBox.size = _boxCollider.size;

    _holdingVector2.Set(_boxCollider.offset.x, _boxCollider.offset.y - 0.5f);
    _shortBox.offset = _holdingVector2;

    _holdingVector2.Set(_boxCollider.size.x, _boxCollider.size.y - 1);
    _shortBox.size = _holdingVector2;
  }

  private void OnEnable() {
    _samusInput.longJumpPressed.OnChange += longJumpInputChanged;
    _samusState.jumpState.OnChange += jumpStateChanged;
    _samusState.isMorphball.OnChange += toggleMorphballCollider;
    _samusState.isInvulnerable.OnChange += ToggleInvulnerability;
  }

  private void OnDisable() {
    _samusInput.longJumpPressed.OnChange -= longJumpInputChanged;
    _samusState.jumpState.OnChange -= jumpStateChanged;
    _samusState.isMorphball.OnChange -= toggleMorphballCollider;
    _samusState.isInvulnerable.OnChange -= ToggleInvulnerability;
  }

  private void FixedUpdate() {
    // Move Sideways
    if(_samusState.isRunning.value) {
      _holdingVector2.Set((_samusState.isForward.value? 1 : -1) * movementSpeed * Time.deltaTime, 0);
      _rigidbody.position += _holdingVector2;
    }

    if(_samusState.isMorphball.value) {
      _canExitMorphball = !Utils.checkTopCollision(_boxCollider, 1, terrainLayer);
    }
  }

  private void Update() {

    if(_rigidbody.velocity.y != 0 && _samusState.jumpState.value == JumpState.Grounded) {
      _samusState.jumpState.value = JumpState.Falling;
    }

    // Should Jump
    if(_samusState.jumpState.value == JumpState.Grounded) {
      // Long Jump
      if(_canLongJumpAgain && _samusInput.longJumpPressed.value) {
        // Do not longjump Until button has been released and pressed again
        _canLongJumpAgain = false;
        jump();
      }

      // // Short Jump
      // if(_samusInput.shortJumpPressed.value) {
      //   _isShortJumping = true;
      //   jump();
      // }
    }

  }

  // Collision Detection

  // private void OnCollisionEnter2D(Collision2D other) {
  //   // Ignore terrain here
  //   if((other.gameObject.layer.toLayerMask() & terrainLayer) != 0) {
  //     return;
  //   }
  // }

  private void OnCollisionStay2D(Collision2D other) {
    // Only look at terrain collisions
    if((other.gameObject.layer.toLayerMask() & terrainLayer) == 0) {
      return;
    }

    _rigidbody.GetContacts(_holdingContactPoints);
    foreach (var contactPoint in _holdingContactPoints) {
      // Floor Collision
      if(_samusState.jumpState.value == JumpState.Grounded) {
        continue;
      }

      if(contactPoint.normal.y < 0) { // Hit Ceiling
        _samusState.jumpState.value = JumpState.Falling;
      } else if(contactPoint.normal.y > 0) { // Hit Floor
        _samusState.jumpState.value = JumpState.Grounded;
      }
    }
  }

  private void OnCollisionExit2D(Collision2D other) {
    // Only look at terrain collisions
    if((other.gameObject.layer.toLayerMask() & terrainLayer) == 0) {
      return;
    }

    // If the collision was exited while Samus was grounded,
    // then the player walked of the edge of a platform without jumping,
    // or was throw into the air by damage recoil
    if(_samusState.jumpState.value == JumpState.Grounded) {
      _samusState.jumpState.value = JumpState.Falling;
    }
  }

  // Jump Related
  private void jump() {
    _jumpInertiaStopped = false;

    _holdingVector2.Set(0, jumpHeight);
    _rigidbody.AddForce(_holdingVector2, ForceMode2D.Impulse);

    _samusAudio.playJump();

    _samusState.jumpState.value = JumpState.Jumping;
  }

  private void longJumpInputChanged(bool longJumpPressed) {
    handleLongJump(longJumpPressed, _samusState.jumpState.value);
  }

  private void jumpStateChanged(JumpState jumpState) {
    // if(_isShortJumping) {
    //   handleShortJumpEnd(jumpState);
    // }

    handleLongJump(_samusInput.longJumpPressed.value, jumpState);
  }

  // Can Longjump after releasing the button, but ignore if pressed again while in air
  private void handleLongJump(bool longJumpPressed, JumpState jumpState) {
    if(longJumpPressed && jumpState != JumpState.Grounded) {
      _canLongJumpAgain = false;
    } else if(!longJumpPressed) {
      _canLongJumpAgain = true;
      stopJumpInertia();
    }
  }

  // // Stop Short jump when jump state changes
  // private void handleShortJumpEnd(JumpState jumpState) {
  //   if(jumpState == JumpState.Falling) {
  //     _isShortJumping = false;
  //     stopJumpInertia();
  //   }
  // }

  private void stopJumpInertia() {
    // Jump button released while on air, after required part of the jump has ended and samus is still going up
    if(!_jumpInertiaStopped && _samusState.jumpState.value == JumpState.Falling && _rigidbody.velocity.y > 0) {
      _rigidbody.velocity = Vector2.zero;
      _jumpInertiaStopped = true;
    }
  }

  // Morphball
  private void toggleMorphballCollider(bool isMorphball) {
    _boxCollider.offset = isMorphball? _shortBox.offset : _tallBox.offset;
    _boxCollider.size = isMorphball? _shortBox.size : _tallBox.size;
    if(!isMorphball) {
      doMorphballHop();
    }
  }

  public void doMorphballHop() {
    // Hop on transition
    _holdingVector3.Set(transform.position.x, transform.position.y + (_samusState.isMorphball.value? toMorphballHop : fromMorphballHop), transform.position.z);

    transform.position = _holdingVector3;
  }

  public bool canSwitchMorphballMode() {
    // Only change morphball state when grounded and standing still
    if(_samusState.jumpState.value != JumpState.Grounded || _samusState.isRunning.value) {
      return false;
    }

    // Do not exit if theres not enough space above
    if(_samusState.isMorphball.value && !_canExitMorphball) {
      return false;
    }

    return true;
  }

  // Damage Related
  public override bool OnDamage(int damage, GameObject damageSource) {
    if(base.OnDamage(damage, damageSource)) {
      return true;
    }

    _samusAudio.playHurt();

    // Make Invulnerable
    _samusState.isInvulnerable.value = true;

    // Recoil samus away from source of damage
    StartCoroutine(recoil(damageSource.transform.position));

    return false;
  }

  private IEnumerator recoil(Vector2 damageSourcePosition) {
      yield return new WaitForFixedUpdate();

      // TODO: rework recoil, implement better prevention

      // Prevent recoil and jump impulses from happening at the same time
      if(_samusState.jumpState.value == JumpState.Grounded) {

        var direction = _rigidbody.position.x > damageSourcePosition.x? Vector2.right : Vector2.left;
        var forceVector = ((_rigidbody.position - damageSourcePosition).normalized * recoilStrength);
        if(Vector2.Angle(forceVector, direction) > maxRecoilHorizontalAngle) {
          forceVector = direction * recoilStrength;
        }

        _rigidbody.AddForce(forceVector, ForceMode2D.Impulse);
        #if UNITY_EDITOR
          Debug.DrawLine(_rigidbody.transform.position, (Vector2) _rigidbody.transform.position + forceVector, Color.yellow, 1);
        #endif
      }
  }

  private void ToggleInvulnerability(bool isInvulnerable) {
    Physics2D.IgnoreLayerCollision(samusLayer.toLayer(), enemyDamageLayer.toLayer(), isInvulnerable);
  }
}
