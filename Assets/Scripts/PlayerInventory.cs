using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public List<PickableObject> playerItems;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private PickableObject equippedItem;
    [SerializeField] private int equippedItemIndex = 0;
    [SerializeField] private Transform equippedItemParent;
    [SerializeField] private Transform throwItemParent;
    [SerializeField] private CharacterAnimation characterAnimation;
    [SerializeField] private TextMeshProUGUI pickupPrompt;
    [SerializeField] private HudScript hudScript;
    [SerializeField] private CharacterAudio characterAudio;
    private bool grannyAlive = true;

    private readonly float maxRaycastDistance = 5f;

    private void Start()
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

        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, maxRaycastDistance, layer))
        {
            var pickableItem = hit.transform.GetComponent<PickableObject>();
            var endingTrigger = hit.transform.GetComponent<EndingTrigger>();
            if (pickableItem && !pickableItem.IsPicked)
            {
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                pickupPrompt.text = "Press F to pick up " + hit.transform.gameObject.name;
                if (pickableItem.Price > 0 && pickableItem.Type != PickableObject.ObjectType.Money)
                {
                    pickupPrompt.text = pickupPrompt.text + "\n" + "Price: " + pickableItem.Price.ToString("F2") + "HBC";
                }

                if (ReadActionKey())
                {
                    PickUpItem(hit.transform.gameObject);
                }
            }
            else if (endingTrigger)
            {
                pickupPrompt.text = "Press F to sleep";
                if (ReadActionKey())
                {
                    bool hasPeaSoup = false;
                    bool humanityCompromised = false;

                    for (int i = 0; i < playerItems.Count; i++)
                    {
                        if (playerItems[i].Type == PickableObject.ObjectType.PeaSoup)
                        {
                            hasPeaSoup = true;
                        }
                        if (playerItems[i].Type == PickableObject.ObjectType.Jauheliha || playerItems[i].Type == PickableObject.ObjectType.Lonkero)
                        {
                            humanityCompromised = true;
                        }
                    }

                    hudScript.GetComponent<FadeHandler>().FadeToBlack();

                    if (playerItems.Count < 2)
                    {
                        hudScript.DisplayEndingText(HudScript.EndingState.StarvationEnding);
                    }
                    else if (hasPeaSoup && grannyAlive && !humanityCompromised)
                    {
                        hudScript.DisplayEndingText(HudScript.EndingState.MoralEnding);
                    }
                    else if (!grannyAlive || humanityCompromised)
                    {
                        hudScript.DisplayEndingText(HudScript.EndingState.ImmoralEnding);
                    }
                    else
                    {
                        hudScript.DisplayEndingText(HudScript.EndingState.StarvationEnding);
                    }
                }
            }
        }
        else
        {
            pickupPrompt.text = "";
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
        if (equippedItem != null && equippedItem.GetComponent<PickableObject>().Type != PickableObject.ObjectType.EmptyHand)
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

    private void PickUpItem(GameObject item)
    {
        var pickableObject = item.GetComponent<PickableObject>();

        if (pickableObject.Type == PickableObject.ObjectType.Money)
        {
            item.SetActive(false);
            hudScript.AddMoney(pickableObject.Price);
            characterAudio.PlayPickupAudioClip();
        }
        else
        {

            if (hudScript.currentMoney >= pickableObject.Price)
            {
                hudScript.AddMoney(-pickableObject.Price);
                item.SetActive(false);
                pickableObject.Price = 0f;
                pickableObject.IsPicked = true;
                playerItems.Add(item.GetComponent<PickableObject>());
                characterAudio.PlayPickupAudioClip();
            }
            //Debug.Log("Not enough Hubocoins!");
        }
    }

    public void RemoveItem(PickableObject item)
    {
        if (item.Type == PickableObject.ObjectType.Granny)
        {
            grannyAlive = false;
        }
        playerItems.Remove(item);
        equippedItemIndex = 0;
        equippedItem = null;
        EquipItem();
    }
}
