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

  private Animator animator = null;

  void Awake() {
    animator = GetComponent<Animator>();

    if(animator == null) {
      Debug.LogError("Samus Animator not found!");
      return;
    }
  }

  void OnEnable() {
    SamusState.instance.isRunning.OnChange   += updateAnimatorRunning;
    SamusState.instance.isAiming.OnChange    += updateAnimatorAiming;
    SamusState.instance.isShooting.OnChange  += updateAnimatorShooting;
    SamusState.instance.isMorphball.OnChange += updateAnimatorMorphball;

    SamusState.instance.jumpState.OnChange += updateAnimatorJumpState;
  }

  void OnDisable() {
    SamusState.instance.isRunning.OnChange   -= updateAnimatorRunning;
    SamusState.instance.isAiming.OnChange    -= updateAnimatorAiming;
    SamusState.instance.isShooting.OnChange  -= updateAnimatorShooting;
    SamusState.instance.isMorphball.OnChange -= updateAnimatorMorphball;

    SamusState.instance.jumpState.OnChange -= updateAnimatorJumpState;
  }

  private void updateAnimatorRunning(bool value) {
    animator.SetBool("isRunning", value);
  }

  private void updateAnimatorAiming(bool value) {
    animator.SetBool("isAiming", value);
    animator.SetLayerWeight((int) Layers.Aim, value? 1f : 0f);
    animator.SetLayerWeight((int) Layers.Aim_Shoot, value && SamusState.instance.isShooting.value? 1f : 0f);
  }

  private void updateAnimatorShooting(bool value) {
    animator.SetBool("isShooting", value);
    animator.SetLayerWeight((int) Layers.Shoot, value? 1f : 0f);
    animator.SetLayerWeight((int) Layers.Aim_Shoot, value && SamusState.instance.isAiming.value? 1f : 0f);
  }

  private void updateAnimatorMorphball(bool value) {
    animator.SetBool("isMorphball", value);
  }

  private void updateAnimatorJumpState(JumpState value) {
    if(SamusState.instance.jumpState.value == value) {
      return;
    }

    switch(value) {
      case JumpState.Grounded:
        animator.SetBool("isGrounded", true);
        animator.SetBool("isJumping", false);
        break;

      case JumpState.Jumping:
        animator.SetBool("isGrounded", false);
        animator.SetBool("isGrounded", true);
        break;

      case JumpState.Falling:
        animator.SetBool("isGrounded", false);
        animator.SetBool("isGrounded", false);
        break;
    }
  }
}
