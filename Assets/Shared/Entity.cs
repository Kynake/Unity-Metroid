using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : PhysicsObject {
  public float movementSpeed;
  public int health;

  protected new void Start() {
    base.Start();
  }
}