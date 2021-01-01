using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
  private KeyCode runKey   = KeyCode.RightArrow;
  private KeyCode jumpKey  = KeyCode.Space;
  private KeyCode aimKey   = KeyCode.UpArrow;
  private KeyCode shootKey = KeyCode.LeftShift;

  private SamusState _samusState;

  private void Awake() {
    _samusState = GetComponent<SamusState>();
    if(_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }
  }

  private void Update() {
    _samusState.isRunning.value  = Input.GetKey(runKey);
    _samusState.isAiming.value   = Input.GetKey(aimKey);
    _samusState.isShooting.value = Input.GetKey(shootKey);

    _samusState.jumpState.value = newJumpState(Input.GetKey(jumpKey));
  }

  private JumpState newJumpState(bool jumpKeyPressed) {
    if(_samusState.jumpState.value == JumpState.Grounded && jumpKeyPressed) {
      return JumpState.Jumping;
    }

    return _samusState.jumpState.value;
  }
}
