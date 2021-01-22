using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
  public GameObject explosionPrefab;
  private const int explosionPoolSize = 3;
  private static ObjectPool _explosionPool;

  void Start() {
    // Initialize Static explosions pool
    if(explosionPrefab != null && _explosionPool == null) {
      _explosionPool = new ObjectPool(explosionPrefab, explosionPoolSize);
    }
  }

  public static void spawnExplosion(Vector2 position) {
    var explosion = _explosionPool.getPooledGameObject();
    if(explosion == null) {
      Debug.LogError("No available objects in Explosion pool");
      return;
    }

    explosion.transform.position = position;
    explosion.SetActive(true);
  }
}
