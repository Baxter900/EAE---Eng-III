using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControllerAttacks : MonoBehaviour{
    [SerializeField] [Required]
    [Tooltip("This is the character controller that will use these attacks.")]
    private CharacterControllerDriver driver;

    [SerializeField] [Required]
    [Tooltip("This is the ui that will display these attacks.")]
    private AbilityUI ui;

    [SerializeField] [Required]
    [Tooltip("This is the null attack to return when there is no attack.")]
    private BaseAttack nullAttack;

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    private LinkedList<BaseAttack> availableAttacks = new LinkedList<BaseAttack>();

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    private LinkedListNode<BaseAttack> selectedAttack;

    void OnTriggerEnter(Collider other){
        AbilityPickup pickup = other.GetComponent<AbilityPickup>();

        if(pickup){
            if(pickup.TryPickup()){
                availableAttacks.AddLast(pickup.attack);
                if(availableAttacks.Count == 1){
                    selectedAttack = availableAttacks.First;
                    ChangeAttackOnDriver();
                }
                ui.PopulateImages();
            }
        }
    }

    public bool EnsureListIsSetup(){
        if(availableAttacks.Count <= 0){
            return false;
        }

        if(selectedAttack == null){
            selectedAttack = availableAttacks.First;
        }
        return true;
    }

    public BaseAttack GetSelectedAttack(){
        if(!EnsureListIsSetup()){
            return nullAttack;
        }
        return selectedAttack.Value;
    }

    public BaseAttack GetAdjacentAttack(bool getRight){
        if(!EnsureListIsSetup()){
            return nullAttack;
        }
        return GetAdjacentAttackNode(getRight).Value;
    }

    public LinkedListNode<BaseAttack> GetAdjacentAttackNode(bool getRight){
        if(!EnsureListIsSetup()){
            return null;
        }
        if(getRight){
            return selectedAttack.Next ?? selectedAttack.List.First;
        }else{
            return selectedAttack.Previous ?? selectedAttack.List.Last;
        }
    }

    public void ChangeSelectedAttack(bool changeToRight){
        if(!EnsureListIsSetup()){
            return;
        }
        selectedAttack = GetAdjacentAttackNode(changeToRight);
        ChangeAttackOnUI(changeToRight);
        ChangeAttackOnDriver();
    }

    private void ChangeAttackOnDriver(){
        driver.ChangeAttack(GetSelectedAttack());
    }

    private void ChangeAttackOnUI(bool changeToRight){
        ui.Scroll(!changeToRight, GetAdjacentAttack(changeToRight).attackIcon);
    }
}
