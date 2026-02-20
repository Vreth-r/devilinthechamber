using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public enum StatName {
    MOVEMENT_SPEED,
    DAMAGE_OUTPUT,
    FIRE_SPEED,
    RELOAD_SPEED,
    MAGAZINE_SIZE,
    PERMA_HEALTH,
    JUMP_HEIGHT,
    SLIDE_DISTANCE,
    HEADSHOT_BONUS,
    LADY_PROJECTILE_SPEED,
    DOG_RECOVERY_SPEED,
    BULLET_RANGE,
    LADY_FIRE_RATE,
    LADY_MOVEMENT_SPEED,
    DOG_MOVEMENT_SPEED,
    SLIDE_SPEED,
    SLIDE_COOLDOWN
}

public enum DealType
{
    POSITIVE,
    NEGATIVE
}


public class StatModManager
{
    // stat modifiers
    public static Dictionary<StatName, List<float>> StatModifiers = new Dictionary<StatName, List<float>>
    {
        // Stat                      
        { StatName.MOVEMENT_SPEED,         new List<float>() },
        { StatName.DAMAGE_OUTPUT,          new List<float>() },
        { StatName.FIRE_SPEED,             new List<float>() },
        { StatName.RELOAD_SPEED,           new List<float>() },
        { StatName.MAGAZINE_SIZE,          new List<float>() },
        { StatName.PERMA_HEALTH,           new List<float>() },
        { StatName.JUMP_HEIGHT,            new List<float>() },
        { StatName.SLIDE_DISTANCE,         new List<float>() },
        { StatName.HEADSHOT_BONUS,         new List<float>() },
        { StatName.LADY_PROJECTILE_SPEED,  new List<float>() },
        { StatName.DOG_RECOVERY_SPEED,     new List<float>() },
        { StatName.BULLET_RANGE,           new List<float>() },
        { StatName.LADY_FIRE_RATE,         new List<float>() },
        { StatName.LADY_MOVEMENT_SPEED,    new List<float>() },
        { StatName.DOG_MOVEMENT_SPEED,     new List<float>() },
        { StatName.SLIDE_SPEED,            new List<float>() },
        { StatName.SLIDE_COOLDOWN,         new List<float>() },
    };

    // add a stat modifier, can be timed but idk if that would be used for stats
    public static void AddStatModifier (StatName statName, float modifier)
    {

        StatModifiers[statName].Add(modifier);
        GameManager.Instance.SetStatMod(statName);
        
    }

    public static float GetStatModifier (StatName statName)
    {
        if (statName == StatName.MAGAZINE_SIZE || statName == StatName.PERMA_HEALTH) // bc magazine size is additive
        {
            if (StatModifiers[statName].Count == 0) return 0;
            return StatModifiers[statName].Sum(x => x);
        }

        if (StatModifiers[statName].Count == 0) return 1f;

        float totalMod = 1;
        foreach (float mod in StatModifiers[statName])
        {
            totalMod *= mod;
        }

        return totalMod;
    }

    public static void ResetStatMods()
    {
        foreach (List<float> statMods in StatModifiers.Values)
        {
            statMods.Clear();
        }
    }
}
