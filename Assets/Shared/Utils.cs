using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Utils {

  public class WatchedValue<T> {
    public WatchedValue(T startingValue) {
      _value = startingValue;
    }

    private T _value;
    public delegate void valueChangeAction(T value);
    public event valueChangeAction OnChange;
    public T value {
      get { return _value; }
      set {
        if(!value.Equals(_value)) { // Always same Type, test Value
          _value = value;
          OnChange?.Invoke(value);
        }
      }
    }
  }

  public static bool checkTopCollision(Collider2D collider, float distance, LayerMask layer) => checkCollision(collider, Vector2.up, distance, getTopCollisionFilter(layer));

  private static  RaycastHit2D[] _holdingRaycast = new RaycastHit2D[10];
  private static bool checkCollision(Collider2D collider, Vector2 direction, float distance, ContactFilter2D filter) {
    if(collider != null) {
      return collider.Cast(direction, filter, _holdingRaycast, distance, true) > 0;
    }

    return false;
  }

  private static ContactFilter2D _holdingFilter = new ContactFilter2D() {};
  private static ContactFilter2D getTopCollisionFilter(LayerMask layer) {
    const float topCollisionAngle = -90;

    _holdingFilter.SetLayerMask(layer);
    _holdingFilter.SetNormalAngle(topCollisionAngle - 1, topCollisionAngle + 1);

    return _holdingFilter;
  }
}