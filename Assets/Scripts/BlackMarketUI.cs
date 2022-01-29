using System.Collections;
using System.Collections.Generic;
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

    [Space(10)]
    [Header("Browser buttons")]
    public Button armButton;
    public int armsForSale = 2;
    public int armValue = 10;

    [Space(10)]
    public Button legButton;
    public int legsForSale = 2;
    public int legValue = 10;

    [Space(10)]
    public Button headButton;
    public int headsForSale = 1;
    public int headValue = 30;

    [Space(10)]
    public Button relativeButton;
    public int relativesForSale = 1;
    public int relativeValue = 100;

    // Start is called before the first frame update
    void Start()
    {
        armButton.onClick.AddListener(armClick);
        legButton.onClick.AddListener(legClick);
        headButton.onClick.AddListener(headClick);
        relativeButton.onClick.AddListener(relativeClick);

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
        var cah = GetComponent<CharacterAccessoryHandler>();
        if (relativesForSale > 0)
        {
            relativesForSale--;
            mainCamera.GetComponent<HudScript>().AddMoney(relativeValue);
            //TODO: sold granny, what now
            Debug.Log("Granny sold!");
        }
        relativeButton.interactable = relativesForSale > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkAll())
        {
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
                Debug.Log("Invalid BlackMarketUI.UIState");
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

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, transform.forward, out hit, maxRaycastDistance))
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

        if (readActionKey())
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

        if (readActionKey())
        {
            setState(UIState.Off);
        }
    }

    private bool readActionKey()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }
        return keyboard.fKey.wasPressedThisFrame;
    }

    private void setButtonPromt(bool value)
    {
        if (!checkAll())
        {
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
                setButtonPromt(false);
                setBrowser(false);
                break;
            case UIState.Prompt:
                setButtonPromt(true);
                setBrowser(false);
                break;
            case UIState.Browser:
                setButtonPromt(false);
                setBrowser(true);
                break;
            default:
                Debug.Log("Invalid BlackMarketUI.UIState");
                break;
        }
        currentState = newState;
    }
}
