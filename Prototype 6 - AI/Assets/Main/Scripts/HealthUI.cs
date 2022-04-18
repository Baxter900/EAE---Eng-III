using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour{
    public Image healthImage;

    public Health healthRef;

    void Start(){
        healthRef.healthEvent.AddListener(OnHealthChange);
    }

    public void OnHealthChange(float newHP, float maxHP){
        healthImage.fillAmount = newHP / maxHP;
    }
}
