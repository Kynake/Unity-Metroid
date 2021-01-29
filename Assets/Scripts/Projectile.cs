using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity {

  [HideInInspector] public Vector2 direction;

  public float disableTimeout; // in seconds
  private Coroutine _disableCoroutine;

  protected virtual void OnEnable() {
    _rigidbody.simulated = true;
    _sprites.ForEach(sprite => sprite.enabled = true);

    direction.Normalize();
    _rigidbody.velocity = direction * movementSpeed;

    if(disableTimeout > 0) {
      _disableCoroutine = StartCoroutine(disableInTime());
    }
  }

  protected virtual void OnDisable() {
    if(_disableCoroutine != null) {
      StopCoroutine(_disableCoroutine);
    }
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

  private IEnumerator disableInTime() {
    yield return new WaitForSeconds(disableTimeout);
    OnDestroyProjectile();
  }
}