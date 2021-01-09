using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusAnimation : AnimatedObject {
  private enum Layers { // Corresponds to the layers in 'Assets/Samus/Animations/Samus Animation Controller'
    Base = 0, Stand = Base,
    Aim = 1,
    Shoot = 2,
    Aim_Shoot = 3
  }

  private SamusState _samusState;

  private new void Awake() {
    base.Awake();
    _samusState = GetComponent<SamusState>();
    if (_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }
  }

  private void OnEnable() {
    _samusState.isRunning.OnChange   += updateAnimatorRunning;
    _samusState.isAiming.OnChange    += updateAnimatorAiming;
    _samusState.isShooting.OnChange  += updateAnimatorShooting;
    _samusState.isMorphball.OnChange += updateAnimatorMorphball;

    _samusState.jumpState.OnChange += updateAnimatorJumpState;
  }

  private void OnDisable() {
    _samusState.isRunning.OnChange   -= updateAnimatorRunning;
    _samusState.isAiming.OnChange    -= updateAnimatorAiming;
    _samusState.isShooting.OnChange  -= updateAnimatorShooting;
    _samusState.isMorphball.OnChange -= updateAnimatorMorphball;

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
}
