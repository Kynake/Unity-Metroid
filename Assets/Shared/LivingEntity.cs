using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : Entity {
  public int health; // in hits from the basic cannon

  /**
   * Default take damage function for Living Entities
   */
  public virtual void OnDamage(int damage) {
    print($"{this.name} took {damage} points of damage.");
    health -= damage;
    if(health <= 0) {
      OnDie();
    }
  }

  /**
   * Default die function for Living Entities
   */
  public virtual void OnDie() {
    print($"{this.name} is Dead.");
  }
}