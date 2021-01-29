using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
  public GameObject explosionPrefab;
  public GameObject mainCamera;
  public AudioClip pauseSFX;

  private const int explosionPoolSize = 3;
  private static ObjectPool _explosionPool;

  private static AudioClip _pauseSFX = null;
  private static AudioSource _sharedAudioSource = null;
  private static AudioSource _mainAudioSource = null;

  private void Awake() {

    if(_sharedAudioSource == null) {
      _sharedAudioSource = GetComponent<AudioSource>();
      if(_sharedAudioSource == null) {
        Debug.LogError($"No Audio Source found for {this.name}");
        return;
      }
    }

    if(_mainAudioSource == null) {
      _mainAudioSource = mainCamera.GetComponent<AudioSource>();
      if(_mainAudioSource == null) {
        Debug.LogError($"No Main Audio Source found for {this.name}");
        return;
      }
    }

    if(_pauseSFX == null) {
      _pauseSFX = pauseSFX;
      if(_pauseSFX == null) {
        Debug.LogError($"No Audio Clip found for pauseSFX");
        return;
      }
    }
  }

  private void Start() {
    // Initialize Static explosions pool
    if(explosionPrefab != null && _explosionPool == null) {
      _explosionPool = new ObjectPool(explosionPrefab, explosionPoolSize);
    }
  }

  public static void spawnExplosion(Vector2 position) {
    var explosion = _explosionPool.getPooledGameObject();
    if(explosion == null) {
      #if UNITY_EDITOR
      Debug.LogWarning("No available objects in Explosion pool");
      #endif
      return;
    }

    explosion.transform.position = position;
    explosion.SetActive(true);
  }

  public static void playSound(AudioClip clip) {
    if(clip != null) {
      _sharedAudioSource.PlayOneShot(clip);
    }
  }

  public static void pauseGame() {
    if(_mainAudioSource.isPlaying) {
      _mainAudioSource.Pause();
    } else {
      _mainAudioSource.UnPause();
    }

    Time.timeScale = Time.timeScale == 0? 1 : 0;
    _sharedAudioSource.PlayOneShot(_pauseSFX);
  }
}
