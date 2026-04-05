using UnityEngine;

using UnityEngine.AddressableAssets;

public class ReloadAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        Addressables.LoadAssetsAsync<Deal>("Reload", DeckManager.Instance.OnDealLoaded);
        UIEvents.ForceHUDRefresh();
    }
}
