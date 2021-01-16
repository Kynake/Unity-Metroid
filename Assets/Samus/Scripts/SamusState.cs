using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** States:
 * Stand - Run
 * Ground - Jump - Fall
 * Not - Shooting
 * Not - Aiming
 * Not - Morphball
 */

public enum JumpState {
  Grounded,
  Jumping,
  Falling
}

public class SamusState : MonoBehaviour {

  // Boolean values
  public Utils.WatchedValue<bool> isRunning      = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> isAiming       = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> isMorphball    = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> isForward      = new Utils.WatchedValue<bool>(true);
  public Utils.WatchedValue<bool> isShooting     = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> isInvulnerable = new Utils.WatchedValue<bool>(false);

  // Enum Values
  public Utils.WatchedValue<JumpState> jumpState = new Utils.WatchedValue<JumpState>(JumpState.Grounded);

  // Update Methdos
  public void updateIsRunning      (bool value) => isRunning.value      = value;
  public void updateIsAiming       (bool value) => isAiming.value       = value;
  public void updateIsMorphball    (bool value) => isMorphball.value    = value;
  public void updateIsForward      (bool value) => isForward.value      = value;
  public void updateIsShooting     (bool value) => isShooting.value     = value;
  public void updateIsInvulnerable (bool value) => isInvulnerable.value = value;

  public void updateJumpState(JumpState value) => jumpState.value = value;

}
