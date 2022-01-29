using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudScript : MonoBehaviour
{
    /// <summary>
    /// TODO: modify value based on some crypto currency API
    /// </summary>
    public static float currencyMultiplier = 1f;

    public int currentMoney = 0;
    public TMPro.TextMeshProUGUI hudMoneyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Add money
    /// </summary>
    /// <param name="amount">amount added, can be negative</param>
    public void AddMoney(int amount)
    {
        if (!CheckAll())
        {
            Debug.Log("Broken ref in HUD script");
            return;
        }

        //TODO: check for negative balance
        currentMoney += amount;
        string moneyText;
        if (currencyMultiplier.Equals(1f))
        {
            moneyText = currentMoney.ToString();
        } else
        {
            moneyText = (currentMoney * currencyMultiplier).ToString("F2");
        }

        hudMoneyText.text = moneyText + " Hubocoins";
    }

    private bool CheckAll()
    {
        return hudMoneyText != null;
    }
}
