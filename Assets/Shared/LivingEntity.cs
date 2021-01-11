using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : Entity {
  public int health; // in hits from the basic cannon

  /**
   * Default die function for Living Entities
   * Override for custom behaviour
   */
  public virtual void die() {
    print($"{this.name} is Dead.");
  }
}