using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : Entity {
  public int health; // in hits from the basic cannon

  /**
   * Default take damage function for Living Entities
   * Returns true if the Entity has died as a result of this attack
   */
  public virtual bool OnDamage(int damage, GameObject damageSource) {
    print($"{this.name} took {damage} points of damage from {damageSource.name}.");
    health -= damage;
    if(health <= 0) {
      OnDie();
    }

    return health <= 0;
  }

  /**
   * Default die function for Living Entities
   */
  public virtual void OnDie() {
    print($"{this.name} is Dead.");
  }
}