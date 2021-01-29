using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreeController : Enemy {

  public float launchTimeout;
  [SerializeField] private List<GameObject> _projectileTargets = null;
  [SerializeField] private GameObject _projectilePrefab = null;

  [SerializeField] private GameObject _raycastLeft = null;
  [SerializeField] private GameObject _raycastRight = null;

  private GameObject _enemyFoundState = null;
  private GameObject _enemyFound {
    get { return _enemyFoundState; }
    set {
      _animator.SetBool("isChasing", value != null);
      _enemyFoundState = value;
    }
  }

  private Vector2 _target = Vector2.zero;
  private Vector2 _initialPosition = Vector2.zero;
  private bool _targetReached = false;
  private bool _groundReached = false;
  private float _lerpAmount = 0;

  private static ObjectPool _projectilePool = null;
  private const int _projectilePoolSize = 10;

  private const float _raycastDistance = 30;

  protected override void Start() {
    if(_projectilePool == null) {
      _projectilePool = new ObjectPool(_projectilePrefab, _projectilePoolSize);
    }
    base.Start();
  }

  private void FixedUpdate() {
    if(_groundReached) {
      StartCoroutine(launchProjectiles());
    }
    if(_enemyFound == null) {
      isIdling();
    }
    if(_enemyFound != null) {
      isChasing();
    }
  }

  private void isIdling() {
    var obj = detectEnemy(_raycastLeft);
    if(obj != null) {
      _target = _raycastLeft.transform.position;

    } else {
      obj = detectEnemy(_raycastRight);
      if(obj != null) {
        _target = _raycastRight.transform.position;
      }
    }

    if(obj != null) {
      _enemyFound = obj;
      _lerpAmount = 0;
      _targetReached = false;
      _initialPosition = transform.position;
    }
  }

  private void isChasing() {
    if(!_targetReached) {
      var lerpTarget = Vector2.Lerp(_initialPosition, _target, (_lerpAmount += Time.fixedDeltaTime) * movementSpeed);
      _rigidbody.MovePosition(lerpTarget);

      if(lerpTarget == _target) {
        _targetReached = true;
      }
    } else {
      #if UNITY_EDITOR
      Debug.DrawRay(transform.position, (_enemyFound.transform.position - transform.position).normalized * _raycastDistance, Color.green, 1);
      #endif

      var ray = Physics2D.Raycast(transform.position, (_enemyFound.transform.position - transform.position).normalized, _raycastDistance, terrainLayer);
      if(ray.point.y > transform.position.y) {
        _target = Vector2.down * _raycastDistance;
      } else {
        _target = ray.point;
      }

      _lerpAmount = 0;
      _targetReached = false;
      _initialPosition = transform.position;
    }
  }

  protected override void OnCollisionEnter2D(Collision2D other) {
    print(other.gameObject.name);
    if((other.gameObject.layer.toLayerMask() & terrainLayer) != 0) {
      _targetReached = true;
      _groundReached = true;
    }
  }

  private GameObject detectEnemy(GameObject raycastReference) {
    #if UNITY_EDITOR
    Debug.DrawRay(raycastReference.transform.position, transform.TransformDirection(Vector2.down) * _raycastDistance, Color.red);
    #endif

    var ray = Physics2D.Raycast(raycastReference.transform.position, transform.TransformDirection(Vector2.down), _raycastDistance, (samusLayer | terrainLayer));
    return ray.collider != null && (ray.collider.gameObject.layer.toLayerMask() & samusLayer) != 0? ray.collider.gameObject : null;
  }

  private IEnumerator launchProjectiles() {
    yield return new WaitForSeconds(launchTimeout);
    gameObject.SetActive(false);

    #if UNITY_EDITOR
    var available = _projectilePool.getAvailableAmount();
    if(_projectileTargets.Count > available) {
      Debug.LogWarning($"Not enough projectiles available in {this.name} Projectile Pool. Required: {_projectileTargets.Count}, Available: {available}");
    }
    #endif

    _projectileTargets.ForEach(target => {
      var projectile = _projectilePool.getPooledGameObject();
      projectile.transform.position = transform.position;

      var projectileScript = projectile.GetComponent<Projectile>();
      projectileScript.direction = target.transform.localPosition;
      projectile.SetActive(true);
    });
  }
}
