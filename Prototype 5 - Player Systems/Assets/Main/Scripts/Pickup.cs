using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum PickupType{
    None,
    Ability
}

public class Pickup : MonoBehaviour{
    [SerializeField] [HideIf("@this.GetType() != typeof(Pickup)")]
    private PickupType _type;
    public virtual PickupType Type{
        get{
            return _type;
        }
    }

    private bool hasBeenPickedUp = false;

    public bool TryPickup(){
        if(hasBeenPickedUp){
            return false;
        }else{
            hasBeenPickedUp = true;
            Destroy(this.gameObject);
            return true;
        }
    }
}
