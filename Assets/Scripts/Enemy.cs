using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LivingEntity {
  protected bool _isHurt {
    get { return _animator.GetBool("isHurt"); }
    set { _animator.SetBool("isHurt", value); }
  }

  protected virtual void Start() {
    _isHurt = false;
  }

  public override bool OnDamage(int damage, GameObject damageSource) {
    // Prevent damage if currently in the middle of taking damage
    if(_isHurt) {
      return false;
    }

    if(base.OnDamage(damage, damageSource)) {
      return true;
    }
    _isHurt = true;
    // TODO: Disable physics while hurt

    return false;
  }

  public override void OnDeath() {
    base.OnDeath();
    gameObject.SetActive(false);

    // Spawn Explosion on death location
    GameController.spawnExplosion(transform.position);
  }

  private void clearHurt() => _isHurt = false;
}