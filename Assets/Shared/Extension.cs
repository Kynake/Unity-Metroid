using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

  /**
    * Finds the first layer that is contained in this LayerMask
    * Returns -1 if no layer is contained in this LayerMask
    */
  public static int toLayer(this LayerMask mask) {
    const int bits = sizeof(int) * 8; // 32 bits in an int
    for(int i = 0; i < bits; i++) {
      if((mask >> i & 1) == 1) {
        return i;
      }
    }

    return -1;
  }

  /**
    * Turns layer into LayerMask
    */
  public static LayerMask toLayerMask(this int layer) {
    return (LayerMask) 1 << layer;
  }
}