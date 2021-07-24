using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputMaster Controls;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerInventoryManager playerInventoryManager;

    private void Awake()
    {
        Controls = new InputMaster();
        Controls.Player.Movement.performed += ctx => playerMovement.OnMovementInput(ctx.ReadValue<Vector2>());
        Controls.Player.Movement.canceled += ctx => playerMovement.OnMovementInput(ctx.ReadValue<Vector2>());
        Controls.Player.StartSprint.performed += _ => playerMovement.OnSprintButtonDown();
        Controls.Player.StartSprint.canceled += _ => playerMovement.OnSprintButtonUp();
        Controls.Player.Dash.performed += _ => playerMovement.OnDashButtonPressed();
        Controls.Player.LightAttack.performed += _ => playerCombat.LightAttack();
        Controls.Player.HeavyAttack.performed += _ => playerCombat.HeavyAttack();
        Controls.UI.Inventory.performed += _ => playerInventoryManager.ToggleInventoryInterface();
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
