using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Utils {

  public class WatchedValue<T> {
    public WatchedValue(T startingValue) {
      _value = startingValue;
    }
    private System.Type type = typeof(T); // Store type to diferentiate between primitives/enums and objects

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

  public enum SideCollision {
    None  = 0b0000,
    Up   = 0b0001,
    Down  = 0b0010,
    Left  = 0b0100,
    Right = 0b1000,
    Walls = Left | Right,
    All   = Up | Down | Walls
  }

  public static SideCollision boxCheckCollision(BoxCollider2D collider, float margin, LayerMask layerMask) {
    var registeredCollisions = SideCollision.None;
    // Up
    registeredCollisions |= Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.up, margin, layerMask).collider == null? SideCollision.None : SideCollision.Up;

    // Down
    registeredCollisions |= Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, margin, layerMask).collider == null? SideCollision.None : SideCollision.Down;

    // Left
    registeredCollisions |= Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.left, margin, layerMask).collider == null? SideCollision.None : SideCollision.Left;

    // Right
    registeredCollisions |= Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.right, margin, layerMask).collider == null? SideCollision.None : SideCollision.Right;

    return registeredCollisions;
  }
}