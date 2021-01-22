using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour, IAnimatedObject {
  // Composition components
  private AnimatedObject _animatedObject = new AnimatedObject();

  // Composition Attributes
  protected Animator _animator;
  protected List<SpriteRenderer> _sprites;

  private void Awake() {
    AwakeAnimatedObject(gameObject, out _animator, out _sprites);
  }

  public void AwakeAnimatedObject(GameObject gameObject, out Animator animator, out List<SpriteRenderer> sprites) {
    _animatedObject.AwakeAnimatedObject(gameObject, out animator, out sprites);
  }

  private void OnExplosionEnd() => gameObject.SetActive(false);
}
