using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMarketUI : MonoBehaviour
{
    public float maxDistance;

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
        if (maxDistance > Vector3.Distance(mainCamera.transform.position, transform.position))
        {
            return;
        }


        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, transform.forward, out hit, maxDistance))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    //Debug.DrawRay(transform.position, transform.forward, Color.red);
                    Debug.Log("hit " + Time.time);
                    setButtonPromt(true);
                }
            }
        }
    }

    public void setButtonPromt(bool value)
    {
        if (!checkAll())
        {
            return;
        }
        buttonPromptText.SetActive(value);
    }

    public void setBrowser(bool value)
    {
        if (!checkAll())
        {
            return;
        }
        buttonPromptText.SetActive(!value);
        browser.SetActive(value);
    }

    private bool checkAll()
    {
        return browser != null && buttonPromptText != null && mainCamera != null;
    }
}
