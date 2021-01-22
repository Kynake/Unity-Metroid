using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IPhysicsObject, IAnimatedObject {
  // Composition components
  private PhysicsObject _physicsObject = new PhysicsObject();
  private AnimatedObject _animatedObject = new AnimatedObject();

  // Composition Attributes
  protected Rigidbody2D _rigidbody;
  protected BoxCollider2D _boxCollider;
  protected BoxCollider2D _boxTrigger;

  protected Animator _animator;
  protected List<SpriteRenderer> _sprites;

  // Entity
  public float movementSpeed; // In tiles per second
  public int damageOnTouch; // in hits from the basic cannon

  // Layers
  public LayerMask terrainLayer;
  public LayerMask samusLayer;
  public LayerMask enemyLayer;

  // Holding Vars
  protected Vector2 _holdingVector2 = Vector2.zero;
  protected Vector3 _holdingVector3 = Vector3.zero;

  // Physics
  protected const int _collisionBufferSize = 10;

  // Methods
  protected virtual void Awake() {
    AwakePhysicsObject(gameObject, out _rigidbody, out _boxCollider, out _boxTrigger);
    AwakeAnimatedObject(gameObject, out _animator, out _sprites);
  }

  public void AwakePhysicsObject(GameObject gameObject, out Rigidbody2D rigidbody, out BoxCollider2D boxCollider, out BoxCollider2D boxTrigger) {
    _physicsObject.AwakePhysicsObject(gameObject, out rigidbody, out boxCollider, out boxTrigger);
  }

  public void AwakeAnimatedObject(GameObject gameObject, out Animator animator, out List<SpriteRenderer> sprites) {
    _animatedObject.AwakeAnimatedObject(gameObject, out animator, out sprites);
  }

  protected virtual void OnCollisionEnter2D(Collision2D other) => OnCollisionOrTrigger2D(other.collider);
  protected virtual void OnTriggerEnter2D(Collider2D other) => OnCollisionOrTrigger2D(other);

  private void OnCollisionOrTrigger2D(Collider2D other) {
    if(damageOnTouch == 0) {
      return;
    }

    var livingEntity = other.gameObject.GetComponent<LivingEntity>();
    if(livingEntity != null) {
      livingEntity.OnDamage(damageOnTouch, gameObject);
    }
  }
}