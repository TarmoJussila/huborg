using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public enum ObjectType
    {
        Default,
        PeaSoup,
        Jauheliha,
        Lonkero,
        Money,
        Granny,
        EmptyHand
    }

    public ObjectType Type = ObjectType.Default;
    public bool IsPicked = false;
    public float Price = 0f;
}
