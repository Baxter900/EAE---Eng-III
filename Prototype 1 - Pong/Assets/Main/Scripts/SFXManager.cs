using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour{
    private static SFXManager _instance = null;

    public static SFXManager Instance{
        get{
            return _instance;
        }

        set{
            if(_instance == null){
                _instance = value;
            }else{
                throw new UnityException("Tried to create a second instance of SFXManager. There can only be one instance of SFXManager at a time.");
            }
        }
    }

    void Awake(){
        _instance = this;
    }

    [SerializeField]
    private AudioClip wallBounceSound;

    [SerializeField]
    private AudioClip paddleBounceSound;

    [SerializeField]
    private AudioClip scoreSound;

    [SerializeField]
    private AudioClip powerupSound;

    [SerializeField]
    private AudioClip powerupSpawnSound;


    private AudioSource audioSource;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWallBounceSound(){
        audioSource.PlayOneShot(wallBounceSound);
    }

    public void PlayPaddleBounceSound(){
        audioSource.PlayOneShot(paddleBounceSound);
    }

    public void PlayScoreBounceSound(){
        audioSource.PlayOneShot(scoreSound);
    }

    public void PlayerPowerupSound(){
        audioSource.PlayOneShot(powerupSound);
    }

    public void PlayerPowerupSpawnSound(){
        audioSource.PlayOneShot(powerupSpawnSound);
    }

}
