using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AbilityPickup : Pickup{

    public override PickupType Type{
        get{
            return PickupType.Ability;
        }
    }

    public BaseAttack attack;
}
