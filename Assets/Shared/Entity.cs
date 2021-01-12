using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : AnimatedObject {
  public float movementSpeed; // In tiles per second
  public int damageOnTouch; // in hits from the basic cannon

  public LayerMask terrainLayer;
  public LayerMask samusLayer;
  public LayerMask enemyLayer;

  protected virtual void OnCollisionEnter2D(Collision2D other) => OnCollisionOrTrigger2D(other.collider);
  protected virtual void OnTriggerEnter2D(Collider2D other) => OnCollisionOrTrigger2D(other);

  private void OnCollisionOrTrigger2D(Collider2D other) {
    if(damageOnTouch == 0) {
      return;
    }

    var livingEntity = other.gameObject.GetComponent<LivingEntity>();
    if(livingEntity != null) {
      livingEntity.OnDamage(damageOnTouch);
    }
  }
}