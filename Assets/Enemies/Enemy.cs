using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity {
  public int damageOnTouch;

  protected new void Awake() {
    base.Awake();
  }
}