using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuInput : MonoBehaviour {
  public string gameSceneName;

  private void OnStartGame(InputValue value) {
    SceneManager.LoadScene(gameSceneName);
  }
}
