using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhysicsObject {
  void AwakePhysicsObject(GameObject gameObject, out Rigidbody2D rigidbody, out BoxCollider2D boxCollider, out BoxCollider2D boxTrigger);
}

public class PhysicsObject: IPhysicsObject {
  public void AwakePhysicsObject(GameObject gameObject, out Rigidbody2D rigidbody, out BoxCollider2D boxCollider, out BoxCollider2D boxTrigger) {
    rigidbody = gameObject.GetComponentInChildren<Rigidbody2D>();
    if(rigidbody == null) {
      Debug.LogError($"RigidBody2D not found in {gameObject.name}");
    }

    boxCollider = null;
    boxTrigger = null;

    var colliders = gameObject.GetComponentsInChildren<BoxCollider2D>(true);
    foreach (var collider in colliders) {
      if(collider.isTrigger) {
        if(boxTrigger == null) {
          boxTrigger = collider;
        } else {
          Debug.LogError($"More than one Trigger found!");
        }
      } else {
        if(boxCollider == null) {
          boxCollider = collider;
        } else {
          Debug.LogError($"More than one Collider found!");
        }
      }
    }

    if(boxCollider == null && boxTrigger == null) {
      Debug.LogError($"No BoxCollider2D's (Trigger or Collider) found in {gameObject.name}");
    }
  }
}