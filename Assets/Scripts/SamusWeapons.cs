using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusWeapons : MonoBehaviour {

  [SerializeField] private GameObject _startingWeapon = null;

  [SerializeField] private GameObject _horizontalAim = null;
  [SerializeField] private GameObject _verticalAim = null;

  private SamusState _samusState;
  private List<ObjectPool> _weaponsPools = new List<ObjectPool>();
  private int _currentWeaponIndex = -1;

  private Coroutine _nextShotScheduled = null;

  private void Awake() {
    _samusState = GetComponent<SamusState>();
    if (_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }
  }

  private void Start() {
    if(_startingWeapon != null) {
      var startingPool = new ObjectPool(_startingWeapon);

      _weaponsPools.Add(startingPool);
      _currentWeaponIndex = 0;
    }
  }

  // private void Update() {

  // }

  // Event functions

  // Called from within animation events
  public void shoot() {
    if(_currentWeaponIndex == -1) {
      print("No Weapons available");
      return;
    }

    Vector2 direction;
    Vector2 position;

    if(_samusState.isAiming.value) {
      direction = Vector2.up;
      position = _verticalAim.transform.position;
    } else {
      direction = _samusState.isForward.value? Vector2.right : Vector2.left;
      position = _horizontalAim.transform.position;
    }

    var beam = _weaponsPools[_currentWeaponIndex].getPooledGameObject();
    if(beam == null) {
      print("No available objects in Projectile pool");
    }

    // Do not start multiple attempts to shoot, let FixedUpdate timer limit the frequency
    if(_nextShotScheduled == null) {
      _nextShotScheduled = StartCoroutine(scheduleNextShot(beam, position, direction));
    }
  }

  private IEnumerator scheduleNextShot(GameObject nextShotObject, Vector2 shotStartLocation, Vector2 shotDirection) {
    // Prevent shot frequency variation by executing them on a fixed timer (that of FixedUpdate)
    yield return new WaitForFixedUpdate();

    nextShotObject.transform.position = shotStartLocation;

    var beamScript = nextShotObject.GetComponent<Projectile>();
    beamScript.direction = shotDirection;

    nextShotObject.SetActive(true);

    // Allow further shots
    _nextShotScheduled = null;
  }

  public void switchWeapon() {
    if(_currentWeaponIndex == -1) {
      return;
    }

    // Get next weapon circularly from list
    _currentWeaponIndex  = (_currentWeaponIndex + 1) % _weaponsPools.Count;
  }
}
