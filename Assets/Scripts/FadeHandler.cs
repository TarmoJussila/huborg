using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeHandler : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    private readonly float defaultIntensity = 0.2f;
    private readonly float maxIntensity = 1.0f;
    private readonly float transitionSpeedMultiplier = 0.5f;
    private readonly float transitionReturnDelay = 3f;

    private float timer;
    private Coroutine fadeCoroutine = null;
    
    private void Awake()
    {
        FadeToDefault();
    }

    public void FadeToBlack()
    {
        if (fadeImage != null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeToBlackCoroutine());
        }
    }

    public void FadeToDefault()
    {
        if (fadeImage != null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeToDefaultCoroutine());
        }
    }

    public void FadeToBlackAndBackToDefault()
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeToBlackAndBackToDefaultCoroutine());
        }
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * transitionSpeedMultiplier;
            var vignetteIntensity = Mathf.Lerp(defaultIntensity, maxIntensity, timer);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, vignetteIntensity);
            yield return null;
        }
    }

    private IEnumerator FadeToDefaultCoroutine()
    {
        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * transitionSpeedMultiplier;
            var vignetteIntensity = Mathf.Lerp(maxIntensity, defaultIntensity, timer);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, vignetteIntensity);
            yield return null;
        }
    }

    private IEnumerator FadeToBlackAndBackToDefaultCoroutine()
    {
        FadeToBlack();

        yield return new WaitForSeconds(transitionReturnDelay);

        FadeToDefault();
    }
}