using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
  protected Rigidbody2D _rigidbody;

  protected BoxCollider2D _boxCollider = null;
  protected BoxCollider2D _boxTrigger = null;

  // Holding Vars
  protected Vector2 _holdingVector2 = Vector2.zero;
  protected Vector3 _holdingVector3 = Vector3.zero;

  // Physics constants
  protected const int _collisionBufferSize = 10;

  protected void Awake() {
    _rigidbody = GetComponentInChildren<Rigidbody2D>();
    if(_rigidbody == null) {
      Debug.LogError($"RigidBody2D not found in {this.name}");
    }

    var colliders = GetComponentsInChildren<BoxCollider2D>(true);
    foreach (var collider in colliders) {
      if(collider.isTrigger) {
        if(_boxTrigger == null) {
          _boxTrigger = collider;
        } else {
          Debug.LogError($"More than one Trigger found!");
        }
      } else {
        if(_boxCollider == null) {
          _boxCollider = collider;
        } else {
          Debug.LogError($"More than one Collider found!");
        }
      }
    }

    if(_boxCollider == null && _boxTrigger == null) {
      Debug.LogError($"No BoxCollider2D's (Trigger or Collider) found in {this.name}");
    }
  }
}
