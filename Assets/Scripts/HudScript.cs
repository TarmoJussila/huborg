using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HudScript : MonoBehaviour
{
    /// <summary>
    /// TODO: modify value based on some crypto currency API
    /// </summary>
    public static float currencyMultiplier = 1f;

    public float currentMoney = 0;
    public TMPro.TextMeshProUGUI hudMoneyText;

    public float escHoldSeconds;
    private float escHoldTime = 0;

    private void Start()
    {
        hudMoneyText.text = CoinsToText(currentMoney) + " HBC";
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            Debug.LogError("Current keyboard is null");
            escHoldTime = 0;
            return;
        }
        if (keyboard.escapeKey.isPressed)
        {
            escHoldTime += Time.deltaTime;
        }
        else
        {
            escHoldTime = 0;
        }

        if (escHoldTime > escHoldSeconds)
        {
            Debug.Log("Loading scene: Playground");
            SceneManager.LoadScene("MainMenu");
        }
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
        hudMoneyText.text = CoinsToText(currentMoney) + " HBC";
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