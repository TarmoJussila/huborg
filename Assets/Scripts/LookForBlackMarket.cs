using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForBlackMarket : MonoBehaviour
{
    public float maxDistance;
    public GameObject blackMarketTerminal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (blackMarketTerminal == null)
        {
            return;
        }
        if (maxDistance > Vector3.Distance(blackMarketTerminal.transform.position, transform.position)) {
            return;
        }


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == blackMarketTerminal)
                {
                    Debug.DrawRay(transform.position, transform.forward, Color.red);
                    Debug.Log("hit " + Time.time);
                }
            }
        }
    }
}
