using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class HealthChangeEvent : UnityEvent<float, float>{}

public class Health : MonoBehaviour{
    [SerializeField]
    private float maxHP;

    [SerializeField]
    private string[] tagWhichAllowDamage;

    [SerializeField]
    private float deathAnimDuration = 0.6f;
    [SerializeField]
    private AnimationCurve deathAnimCurve;
    [SerializeField]
    private Vector3 relativeDeathAnimVelocity = Vector3.up + Vector3.back + Vector3.right;
    [SerializeField]
    private Vector3 relativeDeathAnimAngularVelocity = Vector3.zero;

    public HealthChangeEvent healthEvent = new HealthChangeEvent();

    public UnityEvent deathEvent = new UnityEvent();

    [SerializeField] [ReadOnly] [HideInEditorMode]
    private float currentHP;

    private bool isAlive = true;

    public bool IsAlive{
        get{
            return isAlive;
        }
    }

    void Start(){
        currentHP = maxHP;
    }

    public void Hit(float amount, string[] damageTags){
        if(!isAlive){
            return;
        }

        foreach(string tag in damageTags){
            foreach(string compareTag in tagWhichAllowDamage){
                if(tag == compareTag){
                    Damage(amount);
                    return;
                }
            }
        }
    }

    public void SetTags(string[] damageTags){
        tagWhichAllowDamage = damageTags;
    }

    private void Damage(float amount){
        if(!isAlive){
            return;
        }

        currentHP -= amount;

        healthEvent.Invoke(currentHP, maxHP);

        if(currentHP <= 0f){
            Die();
        }
    }

    private void Die(){
        isAlive = false;
        deathEvent.Invoke();
        DeathAnim(deathAnimDuration);
    }

    private void DeathAnim(float duration){
        // Add the shrink over time component
        gameObject.AddComponent<ShrinkOverTime>().Initialize(duration, 0f, deathAnimCurve);

        Rigidbody rb = GetComponent<Rigidbody>();

        if(rb == null){
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
        // Set velocity
        Vector3 modifiedVelocity = relativeDeathAnimVelocity;
        if(Random.value < 0.5f){
            // 50% chance to go either direction
            modifiedVelocity.x *= -1;
        }
        rb.velocity = transform.TransformVector(modifiedVelocity);
        rb.angularVelocity = transform.TransformVector(relativeDeathAnimAngularVelocity);

        // Destroy after a few seconds
        Destroy(gameObject, duration);
    }
}
