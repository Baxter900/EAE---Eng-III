using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour{
    private static SFXManager _instance = null;
    public static SFXManager Instance{
        get{
            return _instance;
        }
        set{
            Debug.Assert(_instance == null, "Tried to create a second SFXManager. There can only be one SFXManager at a time.");
            _instance = value;
        }
    }

    void Awake(){
        Instance = this;
    }

    public AudioSource audioSource;

    public AudioClip moveNoise;
    public AudioClip winNoise;
    public AudioClip undoNoise;
    public AudioClip redoNoise;
    public AudioClip actionFailedNoise;


    public static void MoveSFX(){
        Instance.audioSource.PlayOneShot(Instance.moveNoise, 1.0f);
    }

    public static void WinSFX(){
        Instance.audioSource.PlayOneShot(Instance.winNoise, 1.0f);
    }

    public static void UndoSFX(){
        Instance.audioSource.PlayOneShot(Instance.undoNoise, 1.0f);
    }

    public static void RedoSFX(){
        Instance.audioSource.PlayOneShot(Instance.redoNoise, 1.0f);
    }

    public static void FailedSFX(){
        Instance.audioSource.PlayOneShot(Instance.actionFailedNoise, 0.5f);
    }
}
