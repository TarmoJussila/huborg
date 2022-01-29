using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Start is called before the first frame update
    void Start()
    {

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
        return browser != null && buttonPromptText != null && mainCamera != null;
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
