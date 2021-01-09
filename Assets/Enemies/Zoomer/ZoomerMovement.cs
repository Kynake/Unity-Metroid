using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomerMovement : MonoBehaviour {
  public bool moveLeft;
  public float walkSpeed;
  // public LayerMask samusLayer;

  public int healh;
  public int damage;

  public LayerMask terrainLayer;
  // public int damagePerHit;

  private Vector2 _moveDirection;
  private List<ContactPoint2D> _currentContactPoints = new List<ContactPoint2D>();

  private const float rotationAngle = 90f;
  private const float collisionMargin = 0.05f;

  // Components
  private Rigidbody2D _rigidBody;
  private BoxCollider2D _boxCollider;

  private bool _ignoringNoGround = false;

  // Holding vars
  private Vector2 _holdingVector2 = Vector2.zero;
  private Vector3 _holdingVector3 = Vector3.zero;
  private List<ContactPoint2D> _holdingContactPoints = new List<ContactPoint2D>();
  private RaycastHit2D _holdingRaycast;

  private void Awake() {
    _rigidBody = GetComponent<Rigidbody2D>();
    if(_rigidBody == null) {
      Debug.LogError("Zoomer Rigidbody2D not found!");
    }

    _boxCollider = GetComponent<BoxCollider2D>();
    if(_boxCollider == null) {
      Debug.LogError("Zoomer BoxCollider2D not found!");
    }
  }

  private void Start() {
    _moveDirection = moveLeft? Vector2.left : Vector2.right;
  }

  private void FixedUpdate() {
    if(stickToGroundSurface()) {
     move();
     detectRotation();
    }
  }

  private bool stickToGroundSurface() {
    if(_rigidBody.GetContacts(_currentContactPoints) == 0) {
      _holdingVector3 = transform.TransformDirection(Vector3.down);
      _rigidBody.position += (Vector2) _holdingVector3 * walkSpeed * Time.deltaTime;

      return false;
    }

    _ignoringNoGround = false;
    return true;
  }

  private void move() {
    _rigidBody.position += (Vector2) transform.TransformDirection(_moveDirection) * walkSpeed * Time.deltaTime;
  }

  private void detectRotation() {
    // Detect 90 degrees angles
    detectInnerRotation();
  }

  private void detectInnerRotation() {
    #if UNITY_EDITOR
    Debug.DrawRay(_boxCollider.bounds.center, transform.TransformDirection(_moveDirection) * (_boxCollider.bounds.extents.x + collisionMargin), Color.green);
    #endif

    _holdingRaycast = Physics2D.Raycast(_boxCollider.bounds.center, transform.TransformDirection(_moveDirection), _boxCollider.bounds.extents.x + collisionMargin, terrainLayer);

    if(_holdingRaycast.collider != null) { // Should rotate
      transform.Rotate(0, 0, (moveLeft? -rotationAngle : rotationAngle), Space.Self);
    }
  }

  private void OnCollisionExit2D(Collision2D other) {
    if(_ignoringNoGround) {
      return;
    }

    transform.Rotate(0, 0, moveLeft? rotationAngle : -rotationAngle, Space.Self);
    _ignoringNoGround = true;
  }
}
