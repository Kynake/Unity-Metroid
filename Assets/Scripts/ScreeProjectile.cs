using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreeProjectile : Projectile {
  protected override void OnTriggerEnter2D(Collider2D other) {
    // _sprites.ForEach(sprite => sprite.enabled = false);

    // Ignore friendly fire collisions with enemies
    if((other.gameObject.layer.toLayerMask() & enemyLayer) != 0) {
      return;
    }

    base.OnTriggerEnter2D(other);
    // _sprites.ForEach(sprite => sprite.enabled = true);
  }
}
