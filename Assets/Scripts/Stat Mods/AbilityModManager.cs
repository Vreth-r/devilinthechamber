using System.Collections.Generic;
using UnityEngine;

public class AbilityModManager : MonoBehaviour
{
    public static AbilityModManager Instance;

    // all abilities
    public Dictionary<AbilityName, AbilityBase> abilities = new Dictionary<AbilityName, AbilityBase> ()
    {
        { AbilityName.INFINITE_MAG, new InfiniteMagAbility() },
        { AbilityName.INVINCIBILITY, new InvincibilityAbility() },
        { AbilityName.FULL_AUTO, new FullAutoAbility() },
        { AbilityName.AOE_RELOAD, new ReloadAOEAbility() },
        { AbilityName.KILL_BULLET_RESTORE, new BulletRestoreAbility() },
        { AbilityName.BLINDNESS, new BlindnessAbility() },
        { AbilityName.EXPLODING_ENEMIES, new ExplodingEnemiesAbility()},
        { AbilityName.PHANTOM_NOISES, new PhantomNoisesAbility()}
    };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartAbility (AbilityName abilityName)
    {   
        abilities[abilityName].initialize(); // set name (i dont think this needs to exist)
        abilities[abilityName].startFunction(); // run the start function (the effect)
        TimerHandler.Instance.CreateTimerHandle(abilities[abilityName].length, abilities[abilityName].endFunction); // set timer to remove effect
    }
}
