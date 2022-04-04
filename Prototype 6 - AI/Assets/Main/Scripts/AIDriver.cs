using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class AIBlackboard{

    public float genericT;
    public float genericTime;

    public Dictionary<string, float> genericFloats;
    public Dictionary<string, bool> genericBools;

    public Vector3 originalLocation;
    public Vector3 desiredLocation;

    public Transform targetEnemy;

    public void Init(){
        genericFloats = new Dictionary<string, float>();
        genericBools = new Dictionary<string, bool>();
    }
}

public class AIDriver : MonoBehaviour{
    [SerializeField]
    public CharacterControllerDriver characterDriver;

    [SerializeField]
    public Rigidbody rb;

    [SerializeField]
    private AIState defaultState;

    [SerializeField]
    private AIState[] allStates;

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    public AIState activeState;

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    public AIBlackboard blackboard;

    void Start(){
        blackboard.Init();
        
        activeState = defaultState;
        activeState.OnEnter(this);

        blackboard.originalLocation = rb.position;
    }

    void Update(){
        CheckTransitionStates();

        activeState.OnUpdate(this);
    }

    private void CheckTransitionStates(){
        if(activeState.ShouldExit(this)){
            for(int i = 0; i < allStates.Length; i++){
                if(allStates[i].ShouldEnter(this)){
                    ChangeState(allStates[i]);
                    return;
                }
            }
        }
    }

    private void ChangeState(AIState newState){
        if(newState == activeState){
            return;
        }

        activeState.OnExit(this);
        activeState = newState;
        activeState.OnEnter(this);
    }

}
