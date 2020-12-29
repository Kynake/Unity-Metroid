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

  private bool _isRunning = false;
  public  bool isRunning {
    get { return _isRunning; }
    set {
      if(_isRunning != value) {
        _isRunning = value;
        animator.SetBool("isRunning", value);
      }
    }
  }

  private bool _isJumping = false;
  public bool isJumping {
    get { return _isJumping; }
    set {
      if(_isJumping != value) {
        _isJumping = value;
        animator.SetBool("isJumping", value);
      }
    }
  }

  private bool _isGrounded = true;
  public bool isGrounded {
    get { return _isGrounded; }
    set {
      if(_isGrounded != value) {
        _isGrounded = value;
        animator.SetBool("isGrounded", value);
      }
    }
  }

  private bool _isAiming = false;
  public bool isAiming {
    get { return _isAiming; }
    set {
      if(_isAiming != value) {
        _isAiming = value;
        animator.SetBool("isAiming", value);
        animator.SetLayerWeight((int) Layers.Aim, value? 1f : 0f);
        animator.SetLayerWeight((int) Layers.Aim_Shoot, value && isShooting? 1f : 0f );
      }
    }
  }

  private bool _isShooting = false;
  public bool isShooting {
    get { return _isShooting; }
    set {
      if(_isShooting != value) {
        _isShooting = value;
        animator.SetBool("isShooting", value);
        animator.SetLayerWeight((int) Layers.Shoot, value? 1f : 0f);
        animator.SetLayerWeight((int) Layers.Aim_Shoot, value && isAiming? 1f : 0f );
      }
    }
  }

  private bool _isMorphball = false;
  public bool isMorphball {
    get { return _isMorphball; }
    set {
      if(_isMorphball != value) {
        _isMorphball = value;
        animator.SetBool("isMorphball", value);
      }
    }
  }

  private KeyCode runKey   = KeyCode.RightArrow;
  // private KeyCode jumpKey  = KeyCode.Space;
  private KeyCode aimKey   = KeyCode.UpArrow;
  private KeyCode shootKey = KeyCode.LeftShift;

  void Awake() {
    animator = GetComponent<Animator>();

    if(animator == null) {
      Debug.LogError("Samus Animator not found!");
      return;
    }


  }

  void Update() {
    isRunning  = Input.GetKey(runKey);
    // isJumping  = Input.GetKey(jumpKey);
    isAiming   = Input.GetKey(aimKey);
    isShooting = Input.GetKey(shootKey);
  }

  public void startFalling() {
    isJumping = false;
  }
}
