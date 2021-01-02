using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SamusInput : MonoBehaviour {
  // Input Status vars
  public Utils.WatchedValue<int> runDirectionPressed = new Utils.WatchedValue<int>(0);

  public Utils.WatchedValue<bool> aimPressed          = new Utils.WatchedValue<bool>(false);

  // Jump
  public Utils.WatchedValue<bool> longJumpPressed  = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> shortJumpPressed = new Utils.WatchedValue<bool>(false);

  // Weapon
  public Utils.WatchedValue<bool> normalShootPressed  = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> fastShootPressed  = new Utils.WatchedValue<bool>(false);


  // Object Components
  private PlayerInput _playerInput;
  private SamusState _samusState;

  // Useful consts
  private const string _defaultActionMap = "Samus Default Controls";
  private const string _morphballActionMap = "Morphball";

  // Holding vars
  private Vector3 _holdingVector = Vector3.zero;

  private void Awake() {
    _playerInput = GetComponent<PlayerInput>();
    if(_playerInput == null) {
      Debug.LogError("Samus PlayerInput not found!");
      return;
    }

    _samusState = GetComponent<SamusState>();
    if(_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }
  }

  // Input Handling
  public void OnRun(InputValue value) {
    // Parse input
    float rawValue = value.Get<float>();
    int direction = 0;
    if(rawValue != 0) {
      direction = rawValue > 0? 1 : -1;
    }

    runDirectionPressed.value = direction;

    _samusState.isRunning.value = direction != 0;

    // Rotate Sprite
    if(direction != 0) {
      _holdingVector.Set(direction, 1, 1);
      transform.localScale = _holdingVector;
      _samusState.isForward.value = direction > 0;
    }
  }

  public void OnAim(InputValue value) {
    aimPressed.value = value.isPressed;
    _samusState.isAiming.value = value.isPressed;
  }

  public void OnEnterMorphball(InputValue value) {
    // Only enter morphball when grounded
    if(_samusState.jumpState.value != JumpState.Grounded || _samusState.isRunning.value) {
      return;
    }
    _playerInput.SwitchCurrentActionMap(_morphballActionMap);
    _samusState.isMorphball.value = true;
  }

  public void OnExitMorphball(InputValue value) {
    _playerInput.SwitchCurrentActionMap(_defaultActionMap);
    _samusState.isMorphball.value = false;
  }

  public void OnSwitchWeapon(InputValue value) {
    print("OnOnSwitchWeaponun");
  }

  public void OnPauseGame(InputValue value) {
    print("OnPauseGame");
  }

  // Jump
  public void OnLongJump(InputValue value) {
    longJumpPressed.value = value.isPressed;
  }

  public void OnShortJump(InputValue value) {
    shortJumpPressed.value = value.isPressed;
  }

  // Shoot
  public void OnNormalShoot(InputValue value) {
    normalShootPressed.value = value.isPressed;
  }

  public void OnFastShoot(InputValue value) {
    fastShootPressed.value = value.isPressed;
  }
}
