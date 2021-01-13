using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool {
  private List<GameObject> _pool;

  // Constructor
  public ObjectPool(GameObject original, int poolSize = 5) {
    _pool = new List<GameObject>(poolSize);

    GameObject tmp;
    for(int i = 0; i < poolSize; i++) {
      tmp = Object.Instantiate(original);
      tmp.SetActive(false);
      _pool.Add(tmp);
    }
  }

  // Methods
  public GameObject getPooledGameObject() => _pool.Find(obj => !obj.activeInHierarchy);
  public int getUsedAmount() => _pool.Count(obj => obj.activeInHierarchy);
  public int getAvailableAmount() => _pool.Count(obj => !obj.activeInHierarchy);

  public T getPooledComponent<T>() where T: class => getPooledGameObject()?.GetComponentInChildren<T>();
}
