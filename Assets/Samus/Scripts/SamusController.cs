using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SamusController : MonoBehaviour {

  public float movementSpeed; // in tiles per second
  public float jumpHeight; // in tiles per second

  private Rigidbody2D _rigidbody;
  private PlayerInput _playerInput;

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

  }

  public void OnRun(InputValue value) {
    print(value.Get<float>());
  }

  public void OnLongJump(InputValue value) {
    print("On Long Jump");
  }

  public void OnShortJump(InputValue value) {
    print(value);
  }

  public void OnAim(InputValue value) {
    print(value.isPressed);
  }

  public void OnNormalShoot(InputValue value) {
    print("OnNormalShoot");
  }

  public void OnFastShoot(InputValue value) {
    print("OnFastShoot");
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

}
