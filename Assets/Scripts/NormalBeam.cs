using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBeam : Projectile {


  protected override void OnEnable() {
    base.OnEnable();

    var rotationAngle = Vector2.Angle(Vector2.right, direction);
    _boxTrigger.gameObject.transform.Rotate(Vector3.forward, rotationAngle, Space.Self);

  }

  protected override void OnTriggerEnter2D(Collider2D other) {
    // Ignore friendly fire collisions with Samus
    if(((1 << other.gameObject.layer) & samusLayer) != 0) {
      return;
    }

    base.OnTriggerEnter2D(other);
  }
}
