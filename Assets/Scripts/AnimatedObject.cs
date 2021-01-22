using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatedObject {
  void AwakeAnimatedObject(GameObject gameObject, out Animator animator, out List<SpriteRenderer> sprites);
}

public class AnimatedObject : IAnimatedObject {
  public void AwakeAnimatedObject(GameObject gameObject, out Animator animator, out List<SpriteRenderer> sprites) {
    animator = gameObject.GetComponentInChildren<Animator>();
    if(animator == null) {
      Debug.LogError($"Missing Animator in {gameObject}");
    }

    sprites = new List<SpriteRenderer>();
    gameObject.GetComponentsInChildren<SpriteRenderer>(sprites);
    if(sprites.Count == 0) {
      Debug.LogError($"No Sprites found in {gameObject.name}");
    }

    animator.keepAnimatorControllerStateOnDisable = false;
  }
}