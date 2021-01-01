using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusController : MonoBehaviour {

  public float movementSpeed = 5.25f; // in tiles per second
  public float jumpHeight; // in tiles per second

  private bool _canLongJumpAgain = true;
  // private bool _isLongJumping = false;
  // private bool _isShortJumping = false;
  private bool _jumpInertiaStopped = false;

  // Object components
  private Rigidbody2D _rigidbody;
  private SamusState _samusState;
  private SamusInput _samusInput;

  // Useful consts
  private int _terrainLayer;
  private int _enemiesLayer;

  // Holding vars
  private Vector2 _holdingVector2 = Vector2.zero;
  private Vector3 _holdingVector3 = Vector3.zero;
  private List<ContactPoint2D> _holdingContactPoints = new List<ContactPoint2D>();

  private void Awake() {
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
    _samusInput.longJumpPressed.OnChange += longJumpInputChanged;
    _samusState.jumpState.OnChange += jumpStateChanged;
  }

  private void OnDisable() {
    _samusInput.longJumpPressed.OnChange -= longJumpInputChanged;
    _samusState.jumpState.OnChange -= jumpStateChanged;
  }

  private void Update() {
    // Move Sideways
    if(_samusState.isRunning.value) {
      _holdingVector3.Set((_samusState.isForward.value? 1 : -1) * movementSpeed * Time.deltaTime, 0, 0);
      transform.position += _holdingVector3;
    }

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
  //   if(other.gameObject.layer == _terrainLayer) {
  //     return;
  //   }
  // }

  private void OnCollisionStay2D(Collision2D other) {
    // Only look at terrain collisions
    if(other.gameObject.layer != _terrainLayer) {
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
    if(other.gameObject.layer != _terrainLayer) {
      return;
    }

    // If the collision was exited while Samus was grounded,
    // then the player walked of the edge of a platform without jumping
    if(_samusState.jumpState.value == JumpState.Grounded) {
      _samusState.jumpState.value = JumpState.Falling;
    }
  }

  // Jump Related
  private void jump() {
    _jumpInertiaStopped = false;

    _holdingVector2.Set(0, jumpHeight);
    _rigidbody.AddForce(_holdingVector2, ForceMode2D.Impulse);

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
}
