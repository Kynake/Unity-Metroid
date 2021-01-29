using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamusAudio : MonoBehaviour {
  public AudioClip jump;
  public AudioClip step;
  public AudioClip shoot;
  public AudioClip hurt;
  public AudioClip morph;

  private AudioSource _audioSource;
  private SamusState _samusState;

  private void Awake() {
    _audioSource = GetComponent<AudioSource>();
    if(_audioSource == null) {
      Debug.LogError("No Audio Source found for Samus");
      return;
    }

    _samusState = GetComponent<SamusState>();
    if (_samusState == null) {
      Debug.LogError("SamusState Script not found!");
      return;
    }
  }

  private void OnEnable() {
    _samusState.isMorphball.OnChange += playMorph;
  }

  private void OnDisable() {
    _samusState.isMorphball.OnChange -= playMorph;
  }

  public void playJump() => playSound(jump);
  public void playStep() => playSound(step);
  public void playShoot() => playSound(shoot);
  public void playHurt() => playSound(hurt);
  public void playMorph(bool isMorphball) {
    if(isMorphball) {
      playSound(morph);
    }
  }

  private void playSound(AudioClip sound) {
    if(sound == null) {
      return;
    }
    _audioSource.clip = sound;
    _audioSource.Play();
  }
}
