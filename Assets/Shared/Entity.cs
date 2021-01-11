using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : AnimatedObject {
  public float movementSpeed; // In tiles per second
  public int damageOnTouch; // in hits from the basic cannon

  public LayerMask terrainLayer;

  protected void OnCollisionEnter2D(Collision2D other) {
    if(damageOnTouch == 0) {
      return;
    }

    var livingEntity = other.gameObject.GetComponent<LivingEntity>();
    if(livingEntity != null) {
      livingEntity.health -= damageOnTouch;
      if(livingEntity.health <= 0) {
        livingEntity.die();
      }
    }
  }
}