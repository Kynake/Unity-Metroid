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
    beam.transform.position = position;

    var beamScript = beam.GetComponent<Projectile>();
    beamScript.direction = direction;

    beam.SetActive(true);
  }

  public void switchWeapon() {
    if(_currentWeaponIndex == -1) {
      return;
    }
    
    // Get next weapon circularly from list
    _currentWeaponIndex  = (_currentWeaponIndex + 1) % _weaponsPools.Count;
  }
}
