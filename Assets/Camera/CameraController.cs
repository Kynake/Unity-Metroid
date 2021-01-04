using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
  public GameObject samus;
  public bool freezeY;

  private Vector3 _holdingVector = Vector3.zero;

  void Update() {
    _holdingVector.Set(samus.transform.position.x, freezeY? transform.position.y : samus.transform.position.y, transform.position.z);
    transform.position = _holdingVector;
  }
}
