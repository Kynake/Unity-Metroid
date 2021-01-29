using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LivingEntity {
  protected bool _isHurt {
    get { return _animator.GetBool("isHurt"); }
    set { _animator.SetBool("isHurt", value); }
  }

  public AudioClip hurt;

  private AudioSource _audioSource;

  protected override void Awake() {
    base.Awake();

    _audioSource = GetComponent<AudioSource>();
    if(_audioSource == null) {
      Debug.LogError($"No Audio Source found for {this.name}");
      return;
    }
  }

  protected virtual void Start() {
    _isHurt = false;
    _audioSource.clip = hurt;
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
    _audioSource.Play(); // SFX on Damage

    // TODO: Disable physics while hurt

    return false;
  }

  public override void OnDeath() {
    base.OnDeath();
    gameObject.SetActive(false);

    // Spawn Explosion and AudioClip at death location
    GameController.spawnExplosion(transform.position);
    GameController.playSound(hurt);
  }

  private void clearHurt() => _isHurt = false;
}