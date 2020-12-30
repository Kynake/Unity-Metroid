using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SamusController : MonoBehaviour
{

  public float movementSpeed; // in tiles per second
  public float jumpHeight; // in tiles per second

//   public PlayerInput playerInput;

  private Rigidbody2D _rigidbody;

  private void Awake() {
    _rigidbody = GetComponent<Rigidbody2D>();
    if (_rigidbody == null)
    {
      Debug.LogError("Samus Rigidbody2D not found!");
      return;
    }
  }

  private void Start() {

  }

  private void Update() {

  }

  public void OnRun(InputAction.CallbackContext value) {
    print("OnRun");
  }

  public void OnLongJump(InputAction.CallbackContext value) {

    print("OnLongJump");
  }

  public void OnShortJump(InputAction.CallbackContext value) {

    print("OnShortJump");
  }

  public void OnAim(InputAction.CallbackContext value) {

    print("OnAim");
  }

  public void OnNormalShoot(InputAction.CallbackContext value) {

    print("OnNormalShoot");
  }

  public void OnFastShoot(InputAction.CallbackContext value) {

    print("OnFastShoot");
  }

  public void OnEnterMorphball(InputAction.CallbackContext value) {

    print("OnOnEnterMorphballRun");
  }

  public void OnExitMorphball(InputAction.CallbackContext value) {

    print("OnExitMorphball");
  }

  public void OnSwitchWeapon(InputAction.CallbackContext value) {

    print("OnOnSwitchWeaponun");
  }

  public void OnPauseGame(InputAction.CallbackContext value) {

    print("OnPauseGame");
  }

}
