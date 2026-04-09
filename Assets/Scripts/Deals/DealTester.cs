using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DealTest
{
    public Deal deal;
    public float activationDelay;
}
public class DealTester : MonoBehaviour
{

    public List<DealTest> dealTests;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < dealTests.Count; i++)
        {
            TimerHandler.Instance.CreateTimerHandle(nameof(i), dealTests[i].activationDelay, dealTests[i].deal.ApplyDeal);
        }
    }
}
