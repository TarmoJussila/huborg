using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlackMarketUI : MonoBehaviour
{
    public enum UIState
    {
        Off,
        Prompt,
        Browser
    }

    public float maxRaycastDistance;
    public float maxProximityDistance;
    public UIState currentState = UIState.Off;
    public GameObject browser;
    public GameObject buttonPromptText;
    public GameObject mainCamera;
    public CharacterAccessoryHandler characterAccessoryHandler;
    public Key openKey;
    public Key closeKey;

    [Space(10)]
    [Header("Browser buttons")]
    public Button armButton;
    public int armsForSale;
    public int armValue;

    [Space(10)]
    public Button legButton;
    public int legsForSale;
    public int legValue;

    [Space(10)]
    public Button headButton;
    public int headsForSale;
    public int headValue;

    [Space(10)]
    public Button relativeButton;
    public int relativesForSale;
    public int relativeValue;

    public List<Image> selections;

    private int currentSelectionId;

    private void Start()
    {
        currentSelectionId = 0;
        for (int i = 0; i < selections.Count; i++)
        {
            selections[i].enabled = false;
        }
        ChangeSelection(currentSelectionId);

        armButton.onClick.AddListener(armClick);
        armButton.GetComponentInChildren<TextMeshProUGUI>().text = "Arm\n" + HudScript.CoinsToText(armValue) + " HBC";
        legButton.onClick.AddListener(legClick);
        legButton.GetComponentInChildren<TextMeshProUGUI>().text = "Leg\n" + HudScript.CoinsToText(legValue) + " HBC";
        headButton.onClick.AddListener(headClick);
        headButton.GetComponentInChildren<TextMeshProUGUI>().text = "Head\n" + HudScript.CoinsToText(headValue) + " HBC";
        relativeButton.onClick.AddListener(relativeClick);
        relativeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Granny\n" + HudScript.CoinsToText(relativeValue) + " HBC";

        if (!checkAll())
        {
            Debug.LogError("Black market ref error");
            return;
        }
    }

    private void armClick()
    {
        if (armsForSale > 0)
        {
            armsForSale--;
            mainCamera.GetComponent<HudScript>().AddMoney(armValue);

            if (armsForSale == 1)
            {
                characterAccessoryHandler.ToggleRightArmAccessory(true);
                characterAccessoryHandler.ToggleLeftArmAccessory(false);
            }
            else if (armsForSale <= 0)
            {
                characterAccessoryHandler.ToggleRightArmAccessory(true);
                characterAccessoryHandler.ToggleLeftArmAccessory(true);
            }
            else
            {
                characterAccessoryHandler.ToggleRightArmAccessory(false);
                characterAccessoryHandler.ToggleLeftArmAccessory(false);
            }
        }
        armButton.interactable = armsForSale > 0;
    }

    private void legClick()
    {
        if (legsForSale > 0)
        {
            legsForSale--;
            mainCamera.GetComponent<HudScript>().AddMoney(legValue);

            if (legsForSale == 1)
            {
                characterAccessoryHandler.ToggleRightLegAccessory(true);
                characterAccessoryHandler.ToggleLeftLegAccessory(false);
            }
            else if (legsForSale <= 0)
            {
                characterAccessoryHandler.ToggleRightLegAccessory(true);
                characterAccessoryHandler.ToggleLeftLegAccessory(true);
            }
            else
            {
                characterAccessoryHandler.ToggleRightLegAccessory(false);
                characterAccessoryHandler.ToggleLeftLegAccessory(false);
            }
        }
        legButton.interactable = legsForSale > 0;
    }

    private void headClick()
    {
        if (headsForSale > 0)
        {
            headsForSale--;
            mainCamera.GetComponent<HudScript>().AddMoney(headValue);
            characterAccessoryHandler.ToggleHeadAccessory(true);
        }
        headButton.interactable = headsForSale > 0;
    }

    private void relativeClick()
    {
        if (relativesForSale > 0)
        {
            relativesForSale--;
            mainCamera.GetComponent<HudScript>().AddMoney(relativeValue);
            //TODO: sold granny, what now
            Debug.Log("Granny sold!");
        }
        relativeButton.interactable = relativesForSale > 0;
    }

    private void Update()
    {
        if (!checkAll())
        {
            Debug.LogError("Black market ref error");
            return;
        }

        switch (currentState)
        {
            case UIState.Off:
                OffUpdate();
                break;
            case UIState.Prompt:
                PromptUpdate();
                break;
            case UIState.Browser:
                BrowserUpdate();
                break;
            default:
                Debug.LogError("Invalid BlackMarketUI.UIState");
                break;
        }
    }

    /// <summary>
    /// Player shouldn't move around when the browser window is open
    /// </summary>
    /// <returns></returns>
    public bool FreezeMovement()
    {
        return currentState == UIState.Browser;
    }

    private void OffUpdate()
    {
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        if (distance <= maxProximityDistance)
        {
            setState(UIState.Prompt);
            return;
        }
        else if (distance > maxRaycastDistance)
        {
            return;
        }

        if (Physics.Raycast(mainCamera.transform.position, transform.forward, out RaycastHit hit, maxRaycastDistance))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {

                    setState(UIState.Prompt);
                }
            }
        }
    }

    private void PromptUpdate()
    {
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        if (distance > maxRaycastDistance)
        {
            setState(UIState.Off);
            return;
        }

        if (readKey(openKey))
        {
            setState(UIState.Browser);
        }
    }

    private void BrowserUpdate()
    {
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        if (distance > maxRaycastDistance)
        {
            setState(UIState.Off);
            return;
        }

        if (readKey(closeKey))
        {
            setState(UIState.Off);
            return;
        }

        if (readKey(openKey))
        {
            var button = selections[currentSelectionId].GetComponentInParent<Button>();
            button.onClick.Invoke();
            return;
        }

        if (readKey(Key.Digit1) || readKey(Key.Numpad1))
        {
            ChangeSelection(0);
        }
        else if (readKey(Key.Digit2) || readKey(Key.Numpad2))
        {
            ChangeSelection(1);
        }
        else if (readKey(Key.Digit3) || readKey(Key.Numpad3))
        {
            ChangeSelection(2);
        }
        else if (readKey(Key.Digit4) || readKey(Key.Numpad4))
        {
            ChangeSelection(3);
        }
    }

    private void ChangeSelection(int selectionId)
    {
        if (selectionId < 0)
        {
            selectionId = 3;
        }
        else if (selectionId > 3)
        {
            selectionId = 0;
        }

        selections[currentSelectionId].enabled = false;
        selections[selectionId].enabled = true;
        currentSelectionId = selectionId;
    }

    private bool readKey(Key key)
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            Debug.LogError("Current keyboard is null");
            return false;
        }
        return keyboard[key].wasPressedThisFrame;
    }

    private void setButtonPrompt(bool value)
    {
        if (!checkAll())
        {
            Debug.LogError("Black market ref error");
            return;
        }
        if (buttonPromptText.activeInHierarchy == value)
        {
            return;
        }
        buttonPromptText.SetActive(value);
    }

    private void setBrowser(bool value)
    {
        if (!checkAll())
        {
            Debug.LogError("Black market ref error");
            return;
        }
        if (browser.activeInHierarchy == value)
        {
            return;
        }
        browser.SetActive(value);
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.None;
    }

    private bool checkAll()
    {
        return browser != null && buttonPromptText != null && mainCamera != null
            && armButton != null && legButton != null && headButton != null
            && relativeButton != null && characterAccessoryHandler != null;
    }

    private void setState(UIState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        switch (newState)
        {
            case UIState.Off:
                setButtonPrompt(false);
                setBrowser(false);
                break;
            case UIState.Prompt:
                setButtonPrompt(true);
                setBrowser(false);
                break;
            case UIState.Browser:
                setButtonPrompt(false);
                setBrowser(true);
                break;
            default:
                Debug.LogError("Invalid BlackMarketUI.UIState");
                break;
        }
        currentState = newState;
    }
}