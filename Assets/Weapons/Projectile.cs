using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Entity {

  [HideInInspector] public Vector2 direction;

  protected virtual void OnEnable() {
    _rigidbody.simulated = true;
    _sprites.ForEach(sprite => sprite.enabled = true);

    direction.Normalize();
    _rigidbody.velocity = direction * movementSpeed;

    var rotationAngle = Vector2.Angle(Vector2.right, direction);
    _boxTrigger.gameObject.transform.Rotate(Vector3.forward, rotationAngle, Space.Self);
  }

  protected virtual void OnDisable() {
    _rigidbody.velocity = Vector2.zero;
    direction = Vector2.right;
  }

  protected override void OnTriggerEnter2D(Collider2D other) {
    base.OnTriggerEnter2D(other);
    _rigidbody.simulated = false;
    _animator.SetBool("OnCollision", true);
  }

  public virtual void OnDestroyProjectile() {
    gameObject.SetActive(false);
    _boxTrigger.transform.rotation = Quaternion.identity;
  }
}