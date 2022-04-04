using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreathAttack", menuName = "Attacks/Breath", order = 1)]
public class BreathAttack : BaseAttack{
    public GameObject particlesPrefab;
    public AudioClip breathAudio;

    public float timeBetweenHits = 0.1f;
    public Capsule[] hitboxes;
    public LayerMask hittableLayers;

    public float damagePerSecond = 10f;
    public string[] damageTags;

    public override void OnSelected(CharacterControllerDriver driver){
        driver.attackState.managedObjs["breath"] = Instantiate(particlesPrefab, driver.attackOrigin);
        driver.attackState.managedObjs["breath"].SetActive(false);
        AudioSource audio = driver.attackState.managedObjs["breath"].AddComponent<AudioSource>();
        audio.clip = breathAudio;
        audio.loop = true;
        driver.attackState.managedComps["audio"] = audio;
    }

    public override void OnDeselected(CharacterControllerDriver driver){
        Destroy(driver.attackState.managedObjs["breath"]);
    }

    public override void OnButtonPressStart(CharacterControllerDriver driver){
        driver.attackState.genericT = timeBetweenHits;
        driver.attackState.managedObjs["breath"].SetActive(true);
        (driver.attackState.managedComps["audio"] as AudioSource).Play();
        // driver.attackState.managedObjs["breath"].GetComponent<ParticleSystem>().Play();
    }

    public override void OnButtonPressEnd(CharacterControllerDriver driver){
        driver.attackState.genericT = 0f;
        driver.attackState.managedObjs["breath"].SetActive(false);
        (driver.attackState.managedComps["audio"] as AudioSource).Stop();

    }

    public override void OnFixedUpdate(CharacterControllerDriver driver){
        driver.attackState.genericT += Time.fixedDeltaTime;
        if(driver.attackState.genericT > timeBetweenHits){
            driver.attackState.genericT -= timeBetweenHits;
            PerformAttack(driver);
        }
    }


    private void PerformAttack(CharacterControllerDriver driver){
        // Get hits
        HashSet<Collider> hitColliders = new HashSet<Collider>();
        Collider[] hitResults = new Collider[100];
        foreach(Capsule hitbox in hitboxes){
            Vector3 hitboxOrientation = (Quaternion.Euler(hitbox.eulerAngles) * driver.attackOrigin.forward).normalized;
            Vector3 hitboxEnd = driver.attackOrigin.position + hitboxOrientation * hitbox.length;
            Physics.OverlapCapsuleNonAlloc(
                driver.attackOrigin.position,
                hitboxEnd,
                hitbox.radius,
                hitResults,
                hittableLayers,
                QueryTriggerInteraction.Collide
            );
            hitColliders.UnionWith(hitResults);

            if(Debug.isDebugBuild){
                DebugExtension.DebugCapsule(
                    driver.attackOrigin.position,
                    hitboxEnd,
                    Color.red,
                    hitbox.radius,
                    timeBetweenHits,
                    true
                );
            }

        }

        // Hit them
        foreach(Collider hit in hitColliders){
            if(hit == null){
                return;
            }
            // Debug.Log(hit);
            Health health = hit.GetComponentInParent<Health>();
            if(health){
                health.Hit(damagePerSecond * timeBetweenHits, damageTags);
            }
        }

    }

}
