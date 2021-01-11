using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomerMovement : Enemy {
  public bool moveLeft;

  private Vector2 _moveDirection;
  private List<ContactPoint2D> _currentContactPoints = new List<ContactPoint2D>(_collisionBufferSize);

  private const float rotationAngle = 90f;
  private const float collisionMargin = 0.05f;
  private const float collisionOffset = 0.0625f;

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
    Debug.DrawRay((Vector2) _boxCollider.bounds.center, transform.TransformDirection(_moveDirection) * (_boxCollider.bounds.extents.x + collisionMargin), Color.green);
    #endif

    var shouldRotate = false;

    var movementRayBottom = Physics2D.Raycast((Vector2) _boxCollider.bounds.center, transform.TransformDirection(_moveDirection), _boxCollider.bounds.extents.x + collisionMargin, terrainLayer);
    shouldRotate = movementRayBottom.collider != null;
    if(!shouldRotate) {
      _holdingVector2.Set(0, collisionOffset);
      _holdingVector2 = (Vector2) transform.TransformDirection(_holdingVector2);

      #if UNITY_EDITOR
      Debug.DrawRay((Vector2) _boxCollider.bounds.center + _holdingVector2, transform.TransformDirection(_moveDirection) * (_boxCollider.bounds.extents.x + collisionMargin), Color.green);
      #endif

      var movementRayTop = Physics2D.Raycast((Vector2) _boxCollider.bounds.center + _holdingVector2, transform.TransformDirection(_moveDirection), _boxCollider.bounds.extents.x + collisionMargin, terrainLayer);
      shouldRotate = movementRayTop.collider != null;
    }


    if(shouldRotate) { // Should rotate
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
