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
        ToggleLeftLegAccessory(enableAllAccessoriesAtStart);
        ToggleRightLegAccessory(enableAllAccessoriesAtStart);
        ToggleLeftArmAccessory(enableAllAccessoriesAtStart);
        ToggleRightArmAccessory(enableAllAccessoriesAtStart);
        ToggleHeadAccessory(enableAllAccessoriesAtStart);
    }

    public void ToggleLeftLegAccessory(bool toggle)
    {
        for (int i = 0; i < leftLegAccessories.Count; i++)
        {
            leftLegAccessories[i].SetActive(toggle);
        }
    }

    public void ToggleRightLegAccessory(bool toggle)
    {
        for (int i = 0; i < rightLegAccessories.Count; i++)
        {
            rightLegAccessories[i].SetActive(toggle);
        }
    }

    public void ToggleLeftArmAccessory(bool toggle)
    {
        for (int i = 0; i < leftArmAccessories.Count; i++)
        {
            leftArmAccessories[i].SetActive(toggle);
        }
    }

    public void ToggleRightArmAccessory(bool toggle)
    {
        for (int i = 0; i < rightArmAccessories.Count; i++)
        {
            rightArmAccessories[i].SetActive(toggle);
        }
    }

    public void ToggleHeadAccessory(bool toggle)
    {
        for (int i = 0; i < headAccessories.Count; i++)
        {
            headAccessories[i].SetActive(toggle);
        }
    }
}