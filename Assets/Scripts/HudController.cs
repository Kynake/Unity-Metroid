using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HudController : MonoBehaviour {
  public Camera mainCamera;
  public Vector2 positionInCamera;

  public List<GameObject> digits;
  public List<Sprite> digitSprites;

  public GameObject player;

  private SamusController _playerScript;
  private List<SpriteRenderer> _renderedDigits;


  private void Start() {
    if(digitSprites.Count != 10) {
      Debug.LogError("Wrong number of Digit sprites in HUD");
      return;
    }

    _renderedDigits = new List<SpriteRenderer>();
    digits.ForEach(digit => _renderedDigits.Add(digit.GetComponent<SpriteRenderer>()));

    _playerScript = player.GetComponent<SamusController>();

    // Reposition HUD relative to camera size
    transform.position = mainCamera.ViewportToWorldPoint(new Vector3(positionInCamera.x, positionInCamera.y, -mainCamera.transform.position.z));

    updateHealth();
  }

  private void Update() => updateHealth();

  private void updateHealth() {
    var health = Mathf.Clamp(_playerScript.health, 0, 99);
    _renderedDigits[0].sprite = digitSprites[health / 10];
    _renderedDigits[1].GetComponent<SpriteRenderer>().sprite = digitSprites[health % 10];
  }
}
