using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusController : MonoBehaviour {

  public float movementSpeed = 5.25f; // in tiles per second
  public float jumpHeight; // in tiles per second

  private bool _canLongJumpAgain = true;
  private bool _isShortJumping = false;
  private bool _jumpInertiaStopped = false;

  // Object components
  private Rigidbody2D _rigidbody;
  private SamusInput  _samusInput;

  // Useful consts
  private int _terrainLayer;
  private int _enemiesLayer;

  // Holding vars
  private Vector2 _holdingVector2 = Vector2.zero;
  private Vector3 _holdingVector3 = Vector3.zero;
  private List<ContactPoint2D> _holdingContactPoints = new List<ContactPoint2D>();

  private void Awake() {
    _samusInput = GetComponent<SamusInput>();
    if (_samusInput == null) {
      Debug.LogError("SamusInput Script not found!");
      return;
    }

    _rigidbody = GetComponent<Rigidbody2D>();
    if (_rigidbody == null) {
      Debug.LogError("Samus Rigidbody2D not found!");
      return;
    }

    // Layers
    _terrainLayer = LayerMask.NameToLayer("Terrain");
    if(_terrainLayer == -1) {
      print("Terrain Layer not Found!");
    }

    _enemiesLayer = LayerMask.NameToLayer("Enemies");
    if(_enemiesLayer == -1) {
      print("Enemies Layer not Found!");
    }
  }

  private void OnEnable() {
    _samusInput.longJumpPressed.OnChange += handleLongJumpInputChange;
    SamusState.instance.jumpState.OnChange += handleShortJumpEnd;
  }

  private void OnDisable() {
    _samusInput.longJumpPressed.OnChange -= handleLongJumpInputChange;
    SamusState.instance.jumpState.OnChange -= handleShortJumpEnd;
  }

  private void Update() {
    // Move Sideways
    if(SamusState.instance.isRunning.value) {
      _holdingVector3.Set((SamusState.instance.isForward.value? 1 : -1) * movementSpeed * Time.deltaTime, 0, 0);
      transform.position += _holdingVector3;
    }

    if(_rigidbody.velocity.y != 0 && SamusState.instance.jumpState.value == JumpState.Grounded) {
      SamusState.instance.jumpState.value = JumpState.Falling;
    }

    // Should Jump
    if(SamusState.instance.jumpState.value == JumpState.Grounded) {
      // Long Jump
      if(_canLongJumpAgain && _samusInput.longJumpPressed.value) {
        // Do not longjump Until button has been released and pressed again
        _canLongJumpAgain = false;
        jump();
      }

      // Short Jump
      if(_samusInput.shortJumpPressed.value) {
        _isShortJumping = true;
        jump();
      }
    }

  }

  // Collision Detection
  private void OnCollisionEnter2D(Collision2D other) {
    // Ignore terrain here
    if(other.gameObject.layer == _terrainLayer) {
      return;
    }
  }

  private void OnCollisionStay2D(Collision2D other) {
    // Only look at terrain collisions
    if(other.gameObject.layer != _terrainLayer) {
      return;
    }

    other.GetContacts(_holdingContactPoints);
    _holdingContactPoints.ForEach(contactPoint => {
      SamusState.instance.isTouchingWall.value = contactPoint.normal.x != 0;
      if(SamusState.instance.jumpState.value == JumpState.Grounded) {
        return;
      }

      if(contactPoint.normal.y < 0) { // Hit Ceiling
        SamusState.instance.jumpState.value = JumpState.Falling;
      } else if(contactPoint.normal.y > 0) { // Hit Floor
        SamusState.instance.jumpState.value = JumpState.Grounded;
      }
    });
  }

  private void OnCollisionExit2D(Collision2D other) {
    // Only look at terrain collisions
    if(other.gameObject.layer != _terrainLayer) {
      return;
    }

    // If the collision was exited while Samus was grounded,
    // then the player walked of the edge of a platform without jumping
    if(SamusState.instance.jumpState.value == JumpState.Grounded) {
      SamusState.instance.jumpState.value = JumpState.Falling;
    }
  }


  // Jump Related
  private void jump() {
    _jumpInertiaStopped = false;

    _holdingVector2.Set(0, jumpHeight);
    _rigidbody.AddForce(_holdingVector2, ForceMode2D.Impulse);

    SamusState.instance.jumpState.value = JumpState.Jumping;
  }

  // Stop longjump when user releases button
  private void handleLongJumpInputChange(bool longJumpPressed) {
    if(!longJumpPressed) {
      _canLongJumpAgain = true;
      stopJumpInertia();
    }
  }

  // Stop Short jump when jump state changes
  private void handleShortJumpEnd(JumpState jumpState) {
    if(_isShortJumping && jumpState == JumpState.Falling) {
      _isShortJumping = false;
      stopJumpInertia();
    }
  }

  private void stopJumpInertia() {
    // Jump button released while on air, after required part of the jump has ended and samus is still going up
    if(!_jumpInertiaStopped && SamusState.instance.jumpState.value == JumpState.Falling && _rigidbody.velocity.y > 0) {
      _rigidbody.velocity = Vector2.zero;
      _jumpInertiaStopped = true;
    }
  }
}
