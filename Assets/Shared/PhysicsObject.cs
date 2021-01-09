using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsObject : MonoBehaviour {
  protected Rigidbody2D _rigidbody;
  protected BoxCollider2D _boxCollider;

  // Holding Vars
  protected Vector2 _holdingVector2;
  protected Vector3 _holdingVector3;

  // Physics constants
  protected const int _collisionBufferSize = 10;

  protected void Start() {
    _rigidbody   = GetComponent<Rigidbody2D>();
    _boxCollider = GetComponent<BoxCollider2D>();

    if(_rigidbody == null) {
      Debug.LogError($"Missing RigidBody2D in {this}");
    }

    if(_boxCollider == null) {
      Debug.LogError($"Missing BoxCollider2D in {this}");
    }
  }
}
