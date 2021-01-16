using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObject : PhysicsObject {

  protected Animator _animator;
  protected List<SpriteRenderer> _sprites = new List<SpriteRenderer>();

  protected new void Awake() {
    base.Awake();
    _animator = GetComponentInChildren<Animator>();
    if(_animator == null) {
      Debug.LogError($"Missing Animator in {this}");
    }

    GetComponentsInChildren<SpriteRenderer>(_sprites);
    if(_sprites.Count == 0) {
      Debug.LogError($"No Sprites found in {this.name}");
    }

    _animator.keepAnimatorControllerStateOnDisable = false;
  }
}