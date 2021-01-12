using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusWeapons : MonoBehaviour {
  public GameObject equippedWeapon;

  [SerializeField] private GameObject _horizontalAim = null;
  [SerializeField] private GameObject _verticalAim = null;

  private SamusState _samusState;

  private void Awake() {
    _samusState = GetComponent<SamusState>();
    if (_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }
  }

  // private void Start() {

  // }

  // private void Update() {

  // }

  // Event functions

  // Called from within animation events
  public void shoot() {
    Vector2 direction;
    Vector2 position;

    if(_samusState.isAiming.value) {
      direction = Vector2.up;
      position = _verticalAim.transform.position;
    } else {
      direction = _samusState.isForward.value? Vector2.right : Vector2.left;
      position = _horizontalAim.transform.position;
    }

    var beam = Instantiate(equippedWeapon, position, Quaternion.identity);
    var beamScript = beam.GetComponent<Projectile>();
    beamScript.direction = direction;
  }

  public void switchWeapon() {
    print("Weapon Switch");
  }
}
