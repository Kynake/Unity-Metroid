using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusAnimation : MonoBehaviour, IAnimatedObject {
  // Composition components
  private AnimatedObject _animatedObject = new AnimatedObject();

  // Composition Attributes
  protected Animator _animator;
  protected List<SpriteRenderer> _sprites;

  private enum Layers { // Corresponds to the layers in 'Assets/Samus/Animations/Samus Animation Controller'
    Base = 0, Stand = Base,
    Aim = 1,
    Shoot = 2,
    Aim_Shoot = 3,
    Invulnerable = 4
  }

  private SamusState _samusState;
  private bool _isFlickering = false;

  private void Awake() {
    AwakeAnimatedObject(gameObject, out _animator, out _sprites);

    _samusState = GetComponent<SamusState>();
    if (_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }
  }

  public void AwakeAnimatedObject(GameObject gameObject, out Animator animator, out List<SpriteRenderer> sprites) {
    _animatedObject.AwakeAnimatedObject(gameObject, out animator, out sprites);
  }

  private void OnEnable() {
    _samusState.isRunning.OnChange      += updateAnimatorRunning;
    _samusState.isAiming.OnChange       += updateAnimatorAiming;
    _samusState.isShooting.OnChange     += updateAnimatorShooting;
    _samusState.isMorphball.OnChange    += updateAnimatorMorphball;
    _samusState.isInvulnerable.OnChange += updateAnimatorInvulnerable;

    _samusState.jumpState.OnChange += updateAnimatorJumpState;
  }

  private void OnDisable() {
    _samusState.isRunning.OnChange      -= updateAnimatorRunning;
    _samusState.isAiming.OnChange       -= updateAnimatorAiming;
    _samusState.isShooting.OnChange     -= updateAnimatorShooting;
    _samusState.isMorphball.OnChange    -= updateAnimatorMorphball;
    _samusState.isInvulnerable.OnChange -= updateAnimatorInvulnerable;

    _samusState.jumpState.OnChange -= updateAnimatorJumpState;
  }

  // Update Animator Functions
  private void updateAnimatorRunning(bool value) {
    _animator.SetBool("isRunning", value);
  }

  private void updateAnimatorAiming(bool value) {
    _animator.SetBool("isAiming", value);
    _animator.SetLayerWeight((int) Layers.Aim, value? 1f : 0f);
    _animator.SetLayerWeight((int) Layers.Aim_Shoot, value && _samusState.isShooting.value? 1f : 0f);
  }

  private void updateAnimatorShooting(bool value) {
    _animator.SetBool("isShooting", value);
    _animator.SetLayerWeight((int) Layers.Shoot, value? 1f : 0f);
    _animator.SetLayerWeight((int) Layers.Aim_Shoot, value && _samusState.isAiming.value? 1f : 0f);
  }

  private void updateAnimatorMorphball(bool value) {
    _animator.SetBool("isMorphball", value);
  }

  private void updateAnimatorJumpState(JumpState value) {
    switch(value) {
      case JumpState.Grounded:
        _animator.SetBool("isGrounded", true);
        _animator.SetBool("isJumping", false);
        break;

      case JumpState.Jumping:
        _animator.SetBool("isGrounded", false);
        _animator.SetBool("isJumping", true);
        break;

      case JumpState.Falling:
        _animator.SetBool("isGrounded", false);
        _animator.SetBool("isJumping", false);
        break;
    }
  }

  private void updateAnimatorInvulnerable(bool value) {
    _animator.SetBool("isInvulnerable", value);
    _animator.SetLayerWeight((int) Layers.Invulnerable, value? 1f : 0f);
  }

  // Unity doesn't allow for aniomations events to functions that take in bools,
  // so this event reads back tha value from the animator
  private void animationEventUpdateInvunerable() {
    if(_isFlickering) {
      return;
    }
    _isFlickering = true;
    StartCoroutine(removeInvulnerability());
  }

  private IEnumerator removeInvulnerability() {
    yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo((int) Layers.Invulnerable).length);
    _isFlickering = false;
    _samusState.isInvulnerable.value = false;
  }
}
