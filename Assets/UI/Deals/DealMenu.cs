using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class DealMenu : MonoBehaviour
{
    UIDocument doc;

    Button deal1;
    Button deal2;
    Button deal3;
    List<Deal> deals;

    void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        deals = DeckManager.Instance.GetRandomDeals();

        deal1 = root.Q<Button>("Deal1");
        deal2 = root.Q<Button>("Deal2");
        deal3 = root.Q<Button>("Deal3");

        deal1.clicked += chooseDeal1;
        deal2.clicked += chooseDeal2;
        deal3.clicked += chooseDeal3;

    }

    void chooseDeal1 ()
    {
        deals[0].ApplyDeal();
    }
    void chooseDeal2 ()
    {
        deals[1].ApplyDeal();
    }
    void chooseDeal3 ()
    {
        deals[2].ApplyDeal();
    }
}
