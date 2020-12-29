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

public sealed class SamusState {
  private static readonly SamusState _instance = new SamusState();

  static  SamusState() {}
  private SamusState() {}

  public static SamusState instance {
    get { return _instance; }
  }

  public WatchedValue<bool> isRunning   = new WatchedValue<bool>(false);
  public WatchedValue<bool> isAiming    = new WatchedValue<bool>(false);
  public WatchedValue<bool> isShooting  = new WatchedValue<bool>(false);
  public WatchedValue<bool> isMorphball = new WatchedValue<bool>(false);

  public WatchedValue<JumpState> jumpState = new WatchedValue<JumpState>(JumpState.Grounded);
}



