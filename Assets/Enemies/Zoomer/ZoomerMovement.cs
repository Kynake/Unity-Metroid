using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomerMovement : Enemy {
  public bool moveLeft;

  private Vector2 _moveDirection;
  private List<ContactPoint2D> _currentContactPoints = new List<ContactPoint2D>(_collisionBufferSize);

  private const float rotationAngle = 90f;
  private const float collisionMargin = 0.05f;

  // Holding vars
  private void Start() {
    _moveDirection = moveLeft? Vector2.left : Vector2.right;
  }

  private void FixedUpdate() {
    if(stickToGroundSurface()) {
     move();
     detectRotation();
    }
  }

  private bool stickToGroundSurface() {
    if(_rigidbody.GetContacts(_currentContactPoints) == 0) {
      _rigidbody.position += (Vector2) transform.TransformDirection(Vector3.down) * movementSpeed * Time.deltaTime;

      return false;
    }

    return true;
  }

  private void move() {
    _rigidbody.position += (Vector2) transform.TransformDirection(_moveDirection) * movementSpeed * Time.deltaTime;
  }

  private void detectRotation() {
    // Detect 90 degrees angles
    detectInnerRotation();
  }

  private void detectInnerRotation() {
    #if UNITY_EDITOR
    Debug.DrawRay(_boxCollider.bounds.center, transform.TransformDirection(_moveDirection) * (_boxCollider.bounds.extents.x + collisionMargin), Color.green);
    #endif

    var movementRay = Physics2D.Raycast(_boxCollider.bounds.center, transform.TransformDirection(_moveDirection), _boxCollider.bounds.extents.x + collisionMargin, terrainLayer);
    if(movementRay.collider != null) { // Should rotate
      transform.Rotate(0, 0, (moveLeft? -rotationAngle : rotationAngle), Space.Self);
    }
  }

  private void OnCollisionExit2D(Collision2D other) {
    // Only look at terrain collisions
    if(((1 << other.gameObject.layer) & terrainLayer) == 0) {
      return;
    }
    
    transform.Rotate(0, 0, moveLeft? rotationAngle : -rotationAngle, Space.Self);
  }
}
