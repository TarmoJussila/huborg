using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public enum ObjectType
    {
        Default,
        Pasta,
        Jauheliha,
        Lonkero,
        Money,
        EmptyHand
    }

    public ObjectType Type = ObjectType.Default;
    public bool IsPicked = false;
    public float Price = 0f;
}
