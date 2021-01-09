using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObject : PhysicsObject {

  protected Animator _animator;

  protected new void Awake() {
    base.Awake();
    _animator = GetComponent<Animator>();
    if(_animator == null) {
      Debug.LogError($"Missing Animator in {this}");
    }
  }
}