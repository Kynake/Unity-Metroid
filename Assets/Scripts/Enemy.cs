using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LivingEntity {
  public override void OnDeath() {
    base.OnDeath();
    gameObject.SetActive(false);

    // Spawn Explosion on death location
    GameController.spawnExplosion(transform.position);
  }
}