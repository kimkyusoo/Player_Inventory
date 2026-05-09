using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public InputAction InteractAction;

    private IInteractable canPickUpItem;
    private PlayerInventory inventory;


    void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    void Start()
    {
        InteractAction.Enable();
    }

    void Update()
    {
        if (InteractAction.WasPressedThisFrame())
        {
            if (canPickUpItem != null)
            {
                canPickUpItem.Pickup(this);
            }
            else
            {
                Debug.Log("주울 아이템이 없습니다.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            canPickUpItem = interactable;
            Debug.Log(interactable.PrintMessage(interactable.ToString()));
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null && canPickUpItem == interactable)
        {
            canPickUpItem = null;
        }
    }



}
