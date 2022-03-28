using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

[System.Serializable]
public struct SFXClip{
    public AudioClip clip;
    public float volume;

    public float startTime;
    public float endTime;

    private bool isClean;

    public SFXClip(AudioClip clip, float volume){
        this.clip = clip;
        this.volume = volume;
        this.isClean = false;
        this.startTime = 0f;
        this.endTime = clip.length;
    }

    public SFXClip(AudioClip clip, float volume, float startTime, float endTime){
        this.clip = clip;
        this.volume = volume;
        this.isClean = false;
        this.startTime = startTime;
        this.endTime = endTime;
    }

    public AudioClip GetClip(){
        if(!this.clip){
            return null;
        }

        if(!isClean){
            if(endTime == 0f){
                endTime = clip.length;
            }
            if(startTime != 0f || endTime != clip.length){
                // Create a new audio clip
                int frequency = clip.frequency;
                float subLength = endTime - startTime;
                int samplesLength = (int)(subLength * frequency * clip.channels);
                AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, clip.channels, frequency, false);
                // Create a temporary buffer for the samples
                float[] data = new float[samplesLength];
                // Get the data from the original clip
                this.clip.GetData(data, (int)(frequency * startTime));
                // Transfer the data to the new clip
                newClip.SetData(data, 0);
                this.clip = newClip;
            }
            isClean = true;
        }
        return this.clip;
    }
}

public class CharacterAudioController : MonoBehaviour{

    [SerializeField]
    private AudioSource primaryAudioSource;

    public void PlayClip(AudioClip clip, float volume = 1f){
        if(!clip){
            // We want to support asking to play a clip without
            return;
        }

        if(primaryAudioSource){
            primaryAudioSource.PlayOneShot(clip, volume);
        }else{
            Debug.LogWarning("Tried to play an audio clip but no audio source is attached to the character.");
        }
    }

    public void PlayClip(SFXClip sfx){
        PlayClip(sfx.GetClip(), sfx.volume);
    }
}
