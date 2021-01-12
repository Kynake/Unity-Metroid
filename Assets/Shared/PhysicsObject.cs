using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
  protected Rigidbody2D _rigidbody;
  protected BoxCollider2D _boxCollider;

  // Holding Vars
  protected Vector2 _holdingVector2 = Vector2.zero;
  protected Vector3 _holdingVector3 = Vector3.zero;

  // Physics constants
  protected const int _collisionBufferSize = 10;

  protected void Awake() {
    _rigidbody   = GetComponentInChildren<Rigidbody2D>();
    _boxCollider = GetComponentInChildren<BoxCollider2D>();

    if(_rigidbody == null) {
      Debug.LogError($"RigidBody2D not found in {this.name}");
    }

    if(_boxCollider == null) {
      Debug.LogError($"BoxCollider2D not found in {this.name}");
    }
  }
}
