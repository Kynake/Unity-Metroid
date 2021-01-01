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

  public Utils.WatchedValue<bool> isRunning      = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> isAiming       = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> isMorphball    = new Utils.WatchedValue<bool>(false);
  public Utils.WatchedValue<bool> isForward      = new Utils.WatchedValue<bool>(true);
  public Utils.WatchedValue<bool> isShooting     = new Utils.WatchedValue<bool>(false);

  public Utils.WatchedValue<JumpState> jumpState = new Utils.WatchedValue<JumpState>(JumpState.Grounded);
}



