using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
  private KeyCode runKey   = KeyCode.RightArrow;
  private KeyCode jumpKey  = KeyCode.Space;
  private KeyCode aimKey   = KeyCode.UpArrow;
  private KeyCode shootKey = KeyCode.LeftShift;

  private void Update() {
    SamusState.instance.isRunning.value  = Input.GetKey(runKey);
    SamusState.instance.isAiming.value   = Input.GetKey(aimKey);
    SamusState.instance.isShooting.value = Input.GetKey(shootKey);

    SamusState.instance.jumpState.value = newJumpState(Input.GetKey(jumpKey));
  }

  private static JumpState newJumpState(bool jumpKeyPressed) {
    if(SamusState.instance.jumpState.value == JumpState.Grounded && jumpKeyPressed) {
      return JumpState.Jumping;
    }

    return SamusState.instance.jumpState.value;
  }
}
