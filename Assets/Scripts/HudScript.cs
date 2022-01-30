using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HudScript : MonoBehaviour
{
    public enum EndingState
    {
        StarvationEnding, ImmoralEnding, MoralEnding
    }

    /// <summary>
    /// TODO: modify value based on some crypto currency API
    /// </summary>
    public static float currencyMultiplier = 1f;

    public float currentMoney = 0;
    public TMPro.TextMeshProUGUI hudMoneyText;
    public TMPro.TextMeshProUGUI endingText;

    public float escHoldSeconds;
    private float escHoldTime = 0;

    private void Start()
    {
        hudMoneyText.text = CoinsToText(currentMoney) + " HBC";
        endingText.text = "";
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

    public void DisplayEndingText(EndingState endingState)
    {
        switch (endingState)
        {
            case EndingState.StarvationEnding:
            {
                endingText.text = "THE END" + System.Environment.NewLine + System.Environment.NewLine + "You died of starvation";
                break;
            }
            case EndingState.ImmoralEnding:
            {
                endingText.text = "THE END" + System.Environment.NewLine + System.Environment.NewLine + "You went to sleep with a full stomach. But at what cost?";
                break;
            }
            case EndingState.MoralEnding:
            {
                endingText.text = "THE END" + System.Environment.NewLine + System.Environment.NewLine + "You were able to fill your stomach and keep your integrity intact";
                break;
            }
            default:
            {
                endingText.text = "THE END";
                break;
            }
        }
    }

    private bool CheckAll()
    {
        return hudMoneyText != null;
    }
}