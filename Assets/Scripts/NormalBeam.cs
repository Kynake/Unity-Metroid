using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBeam : Projectile {
  public float disableTimeout; // in seconds

  private Coroutine _disableCoroutine;

  protected override void OnEnable() {
    base.OnEnable();
    _disableCoroutine = StartCoroutine(disableInTime());
  }

  protected override void OnDisable() {
    if(_disableCoroutine != null) {
      StopCoroutine(_disableCoroutine);
    }
    base.OnDisable();
  }

  protected override void OnTriggerEnter2D(Collider2D other) {
    // Ignore friendly fire collisions with Samus
    if(((1 << other.gameObject.layer) & samusLayer) != 0) {
      return;
    }

    base.OnTriggerEnter2D(other);
  }

  private IEnumerator disableInTime() {
    yield return new WaitForSeconds(disableTimeout);
    OnDestroyProjectile();
  }
}
