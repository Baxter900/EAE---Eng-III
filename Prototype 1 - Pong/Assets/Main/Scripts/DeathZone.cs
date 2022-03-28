using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour{

    [SerializeField]
    [Tooltip("If this is true, this death zone is associated with player 1, otherwise it's associated with player 2.")]
    public bool isPlayer1DeathZone = true;
}
