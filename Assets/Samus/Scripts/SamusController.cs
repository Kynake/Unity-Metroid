using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SamusController : MonoBehaviour {

  public float movementSpeed = 5.25f; // in tiles per second
  public float jumpHeight; // in tiles per second

  // Object components
  private Rigidbody2D _rigidbody;
  private PlayerInput _playerInput;

  // Holding vars
  private Vector3 _holdingVector = Vector3.zero;

  private void Awake() {
    _rigidbody = GetComponent<Rigidbody2D>();
    if (_rigidbody == null) {
      Debug.LogError("Samus Rigidbody2D not found!");
      return;
    }

    _playerInput = GetComponent<PlayerInput>();
    if(_playerInput == null) {
      Debug.LogError("Samus PlayerInput not found!");
      return;
    }
  }

  private void Start() {

  }

  private void Update() {
    if(SamusState.instance.isRunning.value) {
      // Move
      _holdingVector.Set((SamusState.instance.isForward.value? 1 : -1) * movementSpeed * Time.deltaTime, 0, 0);
      transform.position += _holdingVector;
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

    SamusState.instance.isRunning.value = direction != 0;

    // Rotate Sprite
    if(direction != 0) {
      _holdingVector.Set(direction, 1, 1);
      transform.localScale = _holdingVector;
      SamusState.instance.isForward.value = direction > 0;
    }
  }

  public void OnAim(InputValue value) {
    SamusState.instance.isAiming.value = value.isPressed;
  }

  public void OnEnterMorphball(InputValue value) {
    print("OnOnEnterMorphballRun");
  }

  public void OnExitMorphball(InputValue value) {
    print("OnExitMorphball");
  }

  public void OnSwitchWeapon(InputValue value) {
    print("OnOnSwitchWeaponun");
  }

  public void OnPauseGame(InputValue value) {
    print("OnPauseGame");
  }

  // Jump
  public void OnLongJump(InputValue value) {
    OnJump(value, true);
  }

  public void OnShortJump(InputValue value) {
    OnJump(value, false);
  }

  private void OnJump(InputValue value, bool isLongJump) {

  }

  // Shoot
  public void OnNormalShoot(InputValue value) {
    OnShoot(value, false);
  }

  public void OnFastShoot(InputValue value) {
    OnShoot(value, true);
  }

  private void OnShoot(InputValue value, bool isFastShoot) {

  }

}
