using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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