using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity {
  public int damageOnTouch;
  public LayerMask terrainLayer;

  protected new void Start() {
    base.Start();
  }
}