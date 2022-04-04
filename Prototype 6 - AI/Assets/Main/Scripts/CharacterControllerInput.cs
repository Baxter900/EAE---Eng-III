using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class CharacterControllerInput : MonoBehaviour{

    [SerializeField] [Required]
    [Tooltip("This is the controller we should feed the input to.")]
    private CharacterControllerDriver controller;

    [SerializeField] [Required]
    [Tooltip("This is the attacks controller we should feed the input to.")]
    private CharacterControllerAttacks attacks;

    public void OnJump(InputAction.CallbackContext context){
        if(!context.performed){
            return;
        }
        controller.OnJump(context.ReadValueAsButton());
    }

    public void OnDodge(InputAction.CallbackContext context){
        if(!context.performed){
            return;
        }
        controller.OnDodge(context.ReadValueAsButton());
    }

    public void OnMove(InputAction.CallbackContext context){
        controller.OnMove(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context){
        if(!context.performed){
            return;
        }
        controller.OnAttack(context.ReadValueAsButton());
    }

    public void OnToggleAbility(InputAction.CallbackContext context){
        if(!context.performed){
            return;
        }
        attacks.ChangeSelectedAttack(context.ReadValue<float>() < 0f);
    }


}
