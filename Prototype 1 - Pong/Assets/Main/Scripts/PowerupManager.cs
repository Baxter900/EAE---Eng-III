using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour{

    private static PowerupManager _instance = null;

    public static PowerupManager Instance{
        get{
            return _instance;
        }

        set{
            if(_instance == null){
                _instance = value;
            }else{
                throw new UnityException("Tried to create a second instance of PowerupManager. There can only be one instance of PowerupManager at a time.");
            }
        }
    }

    void Awake(){
        _instance = this;
    }


    [SerializeField]
    [Tooltip("The area that powerups can spawn in.")]
    private Vector2 powerupSpawnBounds;

    [SerializeField]
    [Tooltip("A powerup spawns after this many seconds.")]
    private float powerupSpawnInterval = 10f;

    [SerializeField]
    [Tooltip("Powerups despawn after not being used for this long.")]
    public float powerupTimeBeforeDespawn = 30f;

    [SerializeField]
    [Tooltip("The maximum amount of powerups which can be active at once.")]
    private int maxPowerups = 3;

    [SerializeField]
    [Tooltip("The powerup prefabs")]
    private List<Powerup> powerupPrefabs = new List<Powerup>();


    // This is a list of active powerups by their unique name and the active Powerup Component of the powerup GameObject
    private HashSet<Powerup> activePowerupTracker = new HashSet<Powerup>();


    private float timeLeftUntilSpawn = 0f;

    void Start(){
        timeLeftUntilSpawn = powerupSpawnInterval;
    }

    void Update(){
        timeLeftUntilSpawn -= Time.deltaTime;

        if(timeLeftUntilSpawn <= 0f && activePowerupTracker.Count < maxPowerups){
            SpawnPowerup();
            timeLeftUntilSpawn = powerupSpawnInterval;
        }
    }

    private void SpawnPowerup(){
        SFXManager.Instance.PlayerPowerupSpawnSound();
        Powerup powerup = GameObject.Instantiate(GetPowerupPrefab());
        Debug.Assert(powerup != null);
        powerup.transform.position = GetSpawnLocation();
        activePowerupTracker.Add(powerup);
    }

    public void DespawnPowerup(Powerup powerup){
        activePowerupTracker.Remove(powerup);
        GameObject.Destroy(powerup.gameObject);
    }

    private Vector2 GetSpawnLocation(){
        Vector2 position2D = transform.position;
        return position2D + new Vector2(Random.Range(-powerupSpawnBounds.x, powerupSpawnBounds.x), Random.Range(-powerupSpawnBounds.y, powerupSpawnBounds.y));
    }

    private Powerup GetPowerupPrefab(){
        return powerupPrefabs[Random.Range(0, powerupPrefabs.Count)];
    }

}
