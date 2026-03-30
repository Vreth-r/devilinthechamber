using System.Collections.Generic;
using UnityEngine;


public class DealTester : MonoBehaviour
{

    public List<Deal> deals;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < deals.Count; i++)
        {
            deals[i].ApplyDeal();
        }
    }
}
