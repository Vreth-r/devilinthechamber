using UnityEngine;
using UnityEngine.AddressableAssets;
public class BackwardsLogicAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        Addressables.LoadAssetsAsync<Deal>("Backwards Logic", DeckManager.Instance.OnDealLoaded);
    }
}
