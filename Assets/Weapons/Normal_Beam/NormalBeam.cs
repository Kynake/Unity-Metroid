using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBeam : Projectile {
  public float destroyTimeout; // in seconds

  protected override void Start() {
    base.Start();
    StartCoroutine(destroyInTime());
  }

  protected override void OnTriggerEnter2D(Collider2D other) {
    // Ignore friendly fire collisions with Samus
    if(((1 << other.gameObject.layer) & samusLayer) != 0) {
      return;
    }

    base.OnTriggerEnter2D(other);
  }

  IEnumerator destroyInTime() {
    yield return new WaitForSeconds(destroyTimeout);
    Destroy(gameObject);
  }
}
