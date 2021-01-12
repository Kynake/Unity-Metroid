using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Entity {
  [HideInInspector]
  public Vector2 direction;

  protected virtual void Start() {
    direction.Normalize();
    _rigidbody.velocity = direction * movementSpeed;

    var rotationAngle = Vector2.Angle(Vector2.right, direction);
    _boxCollider.gameObject.transform.Rotate(Vector3.forward, rotationAngle, Space.Self);

    print($"dir: {direction}, angle: {rotationAngle}");
  }

  protected override void OnTriggerEnter2D(Collider2D other) {
    base.OnTriggerEnter2D(other);
    Destroy(gameObject);
  }
}