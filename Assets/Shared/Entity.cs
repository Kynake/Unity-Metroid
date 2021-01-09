using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : AnimatedObject {
  public float movementSpeed; // In tiles per second
  public int health; // in hits from the basic Samus

  public LayerMask terrainLayer;
}