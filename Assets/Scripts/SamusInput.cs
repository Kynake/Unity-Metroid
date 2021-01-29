using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SamusInput : MonoBehaviour {
  // Input Status vars
  public Utils.WatchedValue<int> runDirectionPressed = new Utils.WatchedValue<int>(0);

  public Utils.WatchedValue<bool> aimPressed = new Utils.WatchedValue<bool>(false);

  // Jump
  public Utils.WatchedValue<bool> longJumpPressed  = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> shortJumpPressed = new Utils.WatchedValue<bool>(false);

  // Weapon
  public Utils.WatchedValue<bool> normalShootPressed  = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> fastShootPressed  = new Utils.WatchedValue<bool>(false);


  // Object Components
  public GameController gameController;
  private PlayerInput _playerInput;
  private SamusState _samusState;
  private SamusController _samusController;
  private SamusWeapons _samusWeapons;


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

    _samusController = GetComponent<SamusController>();
    if(_samusController == null) {
      Debug.LogError("SamusController Script not found!");
      return;
    }

    _samusWeapons = GetComponent<SamusWeapons>();
    if(_samusWeapons == null) {
      Debug.LogError("SamusWeapons Script not found!");
      return;
    }
  }

  // Input Handling
  public void OnRun(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

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
    if(Time.timeScale == 0) {
      return;
    }

    aimPressed.value = value.isPressed;
    _samusState.isAiming.value = value.isPressed;
  }

  public void OnEnterMorphball(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

    if(!_samusController.canSwitchMorphballMode()) {
      return;
    }

    _playerInput.SwitchCurrentActionMap(_morphballActionMap);
    _samusState.isMorphball.value = true;
  }

  public void OnExitMorphball(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

    if(!_samusController.canSwitchMorphballMode()) {
      return;
    }

    _playerInput.SwitchCurrentActionMap(_defaultActionMap);
    _samusState.isMorphball.value = false;
  }

  public void OnSwitchWeapon(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

    _samusWeapons.switchWeapon();
  }

  public void OnPauseGame(InputValue value) {
    print("OnPauseGame");
    GameController.pauseGame();
  }

  // Jump
  public void OnLongJump(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

    longJumpPressed.value = value.isPressed;
  }

  public void OnShortJump(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

    shortJumpPressed.value = value.isPressed;
  }

  // Shoot
  public void OnNormalShoot(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

    normalShootPressed.value = value.isPressed;
    _samusState.isShooting.value = value.isPressed;
  }

  public void OnFastShoot(InputValue value) {
    if(Time.timeScale == 0) {
      return;
    }

    fastShootPressed.value = value.isPressed;
  }
}
