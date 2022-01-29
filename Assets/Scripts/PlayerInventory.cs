using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<PickableObject> playerItems;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private PickableObject equippedItem;
    [SerializeField] private int equippedItemIndex = 0;
    [SerializeField] private Transform equippedItemParent;
    [SerializeField] private Transform throwItemParent;
    [SerializeField] private CharacterAnimation characterAnimation;
    // Start is called before the first frame update
    void Start()
    {
        var hand = new PickableObject();
        hand.Type = PickableObject.ObjectType.EmptyHand;
        playerItems.Add(hand);
        EquipItem();
    }

    private void EquipItem()
    {
        if (equippedItem != null)
        {
            equippedItem.gameObject.SetActive(false);
        }
        equippedItemIndex = equippedItemIndex < playerItems.Count ? equippedItemIndex : 0;
        equippedItem = playerItems[equippedItemIndex];

        bool isEmptyHand = equippedItem.Type == PickableObject.ObjectType.EmptyHand;

        if (!isEmptyHand)
        {
            equippedItem.transform.SetParent(equippedItemParent);
            equippedItem.transform.localPosition = Vector3.zero;
            equippedItem.transform.localRotation = Quaternion.identity;
            var rb = equippedItem.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
            }
            equippedItem.gameObject.SetActive(true);
        }
        characterAnimation.ToggleRightArmGesture(!isEmptyHand);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        int layer = LayerMask.GetMask("PickableItems");

        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
        {
            var pickableItem = hit.transform.GetComponent<PickableObject>();
            if (pickableItem && !pickableItem.IsPicked)
            {
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                Debug.Log("Can pick: " + hit.transform.gameObject.name);
                if (ReadActionKey())
                {
                    PickUpItem(hit.transform.gameObject);
                }
            }
        }
        else
        {
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }

    private void Update()
    {
        if (ReadThrowKey())
        {
            Throw();
        }
        else if (ReadSwapItemKey())
        {
            equippedItemIndex++;
            EquipItem();
        }
    }

    private void Throw()
    {
        if (equippedItem.GetComponent<PickableObject>().Type != PickableObject.ObjectType.EmptyHand)
        {
            Debug.Log("THROW");

            equippedItem.transform.SetParent(null);
            playerItems.Remove(equippedItem);
            equippedItemIndex = Mathf.Clamp(equippedItemIndex - 1, 0, playerItems.Count - 1);

            var rb = equippedItem.GetComponent<Rigidbody>();
            equippedItem.transform.position = throwItemParent.position + throwItemParent.forward;
            rb.isKinematic = false;
            rb.AddForce(throwItemParent.forward * 500f);
            equippedItem.IsPicked = false;
            equippedItem = null;
            EquipItem();
        }
    }

    private bool ReadSwapItemKey()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }
        return keyboard.eKey.wasPressedThisFrame;
    }

    private bool ReadThrowKey()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }
        return keyboard.gKey.wasPressedThisFrame;
    }

    private bool ReadActionKey()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }
        return keyboard.fKey.wasPressedThisFrame;
    }

    private void PickUpItem(GameObject gameObject)
    {
        //gameObject.SetActive(false);
        var pickableObject = gameObject.GetComponent<PickableObject>();
        pickableObject.IsPicked = true;
        playerItems.Add(gameObject.GetComponent<PickableObject>());
        equippedItemIndex = playerItems.Count - 1;
        EquipItem();
    }
}
