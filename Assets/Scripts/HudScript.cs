using UnityEngine;

public class HudScript : MonoBehaviour
{
    /// <summary>
    /// TODO: modify value based on some crypto currency API
    /// </summary>
    public static float currencyMultiplier = 1f;

    public float currentMoney = 0;
    public TMPro.TextMeshProUGUI hudMoneyText;

    private void Start()
    {
        hudMoneyText.text = CoinsToText(currentMoney) + " Hubocoins";
    }

    /// <summary>
    /// Add money
    /// </summary>
    /// <param name="amount">amount added, can be negative</param>
    public void AddMoney(float amount)
    {
        if (!CheckAll())
        {
            Debug.LogError("Broken ref in HUD script");
            return;
        }
        currentMoney += amount;

        //TODO: check for negative balance
        hudMoneyText.text = CoinsToText(currentMoney) + " Hubocoins";
    }

    public static string CoinsToText(float coins)
    {
        if (currencyMultiplier.Equals(1f))
        {
            return coins.ToString("F2");
        }
        else
        {
            return (coins * currencyMultiplier).ToString("F2");
        }
    }

    private bool CheckAll()
    {
        return hudMoneyText != null;
    }
}