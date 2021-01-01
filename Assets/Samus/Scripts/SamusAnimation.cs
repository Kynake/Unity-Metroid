using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusAnimation : MonoBehaviour {
  private enum Layers { // Corresponds to the layers in 'Assets/Samus/Animations/Samus Animation Controller'
    Base = 0, Stand = Base,
    Aim = 1,
    Shoot = 2,
    Aim_Shoot = 3
  }

  private Animator animator;
  private SamusState _samusState;


  private void Awake() {
    _samusState = GetComponent<SamusState>();
    if (_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }

    animator = GetComponent<Animator>();
    if(animator == null) {
      Debug.LogError("Samus Animator not found!");
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

  private void updateAnimatorRunning(bool value) {
    animator.SetBool("isRunning", value);
  }

  private void updateAnimatorAiming(bool value) {
    animator.SetBool("isAiming", value);
    animator.SetLayerWeight((int) Layers.Aim, value? 1f : 0f);
    animator.SetLayerWeight((int) Layers.Aim_Shoot, value && _samusState.isShooting.value? 1f : 0f);
  }

  private void updateAnimatorShooting(bool value) {
    animator.SetBool("isShooting", value);
    animator.SetLayerWeight((int) Layers.Shoot, value? 1f : 0f);
    animator.SetLayerWeight((int) Layers.Aim_Shoot, value && _samusState.isAiming.value? 1f : 0f);
  }

  private void updateAnimatorMorphball(bool value) {
    animator.SetBool("isMorphball", value);
  }

  private void updateAnimatorJumpState(JumpState value) {
    switch(value) {
      case JumpState.Grounded:
        animator.SetBool("isGrounded", true);
        animator.SetBool("isJumping", false);
        break;

      case JumpState.Jumping:
        animator.SetBool("isGrounded", false);
        animator.SetBool("isJumping", true);
        break;

      case JumpState.Falling:
        animator.SetBool("isGrounded", false);
        animator.SetBool("isJumping", false);
        break;
    }
  }

  private void updateJumpState(JumpState value) {
    _samusState.jumpState.value = value;
  }
}
