using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
  public GameObject explosionPrefab;
  private const int explosionPoolSize = 3;
  private static ObjectPool _explosionPool;

  private static AudioSource _sharedAudioSource = null;

  private void Awake() {

    if(_sharedAudioSource == null) {
      _sharedAudioSource = GetComponent<AudioSource>();
      if(_sharedAudioSource == null) {
        Debug.LogError($"No Audio Source found for {this.name}");
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
}
