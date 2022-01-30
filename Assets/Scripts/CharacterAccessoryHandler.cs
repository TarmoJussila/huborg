using System.Collections.Generic;
using UnityEngine;

public class CharacterAccessoryHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> leftLegAccessories;
    [SerializeField] private List<GameObject> rightLegAccessories;
    [SerializeField] private List<GameObject> leftArmAccessories;
    [SerializeField] private List<GameObject> rightArmAccessories;
    [SerializeField] private List<GameObject> headAccessories;
    [SerializeField] private bool enableAllAccessoriesAtStart = false;

    private void Awake()
    {
        ToggleLeftLegAccessory(enableAllAccessoriesAtStart, false);
        ToggleRightLegAccessory(enableAllAccessoriesAtStart, false);
        ToggleLeftArmAccessory(enableAllAccessoriesAtStart, false);
        ToggleRightArmAccessory(enableAllAccessoriesAtStart, false);
        ToggleHeadAccessory(enableAllAccessoriesAtStart, false);
    }

    public void ToggleLeftLegAccessory(bool toggle, bool playSound = true)
    {
        for (int i = 0; i < leftLegAccessories.Count; i++)
        {
            leftLegAccessories[i].SetActive(toggle);
        }

        if (toggle && playSound)
        {
            GetComponent<CharacterAudio>().PlayUpgradeAudioClip();
        }
    }

    public void ToggleRightLegAccessory(bool toggle, bool playSound = true)
    {
        for (int i = 0; i < rightLegAccessories.Count; i++)
        {
            rightLegAccessories[i].SetActive(toggle);
        }

        if (toggle && playSound)
        {
            GetComponent<CharacterAudio>().PlayUpgradeAudioClip();
        }
    }

    public void ToggleLeftArmAccessory(bool toggle, bool playSound = true)
    {
        for (int i = 0; i < leftArmAccessories.Count; i++)
        {
            leftArmAccessories[i].SetActive(toggle);
        }

        if (toggle && playSound)
        {
            GetComponent<CharacterAudio>().PlayUpgradeAudioClip();
        }
    }

    public void ToggleRightArmAccessory(bool toggle, bool playSound = true)
    {
        for (int i = 0; i < rightArmAccessories.Count; i++)
        {
            rightArmAccessories[i].SetActive(toggle);
        }

        if (toggle && playSound)
        {
            GetComponent<CharacterAudio>().PlayUpgradeAudioClip();
        }
    }

    public void ToggleHeadAccessory(bool toggle, bool playSound = true)
    {
        for (int i = 0; i < headAccessories.Count; i++)
        {
            headAccessories[i].SetActive(toggle);
        }

        if (toggle && playSound)
        {
            GetComponent<CharacterAudio>().PlayUpgradeAudioClip();
        }
    }
}