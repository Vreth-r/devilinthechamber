using UnityEngine;

using UnityEngine.AddressableAssets;

public class HopAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        Addressables.LoadAssetsAsync<Deal>("Hop", DeckManager.Instance.OnDealLoaded);
    }
}
