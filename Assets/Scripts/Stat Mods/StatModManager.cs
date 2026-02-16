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
    JUMP_HEIGHT,
    SLIDE_DISTANCE,
    HEADSHOT_BONUS,
    LADY_PROJECTILE_SPEED,
    DOG_RECOVERY_SPEED
    
}

public enum DealType
{
    POSITIVE,
    NEGATIVE
}

public class StatDescription
{
    public float positiveIncrement;
    public int positiveDealsTaken;
    public List<float> negativeIncrements;
    public int negativeDealsTaken;

    public StatDescription(float posInc, int posDeals, List<float> negIncs, int negDeals)
    {
        positiveIncrement = posInc;
        positiveDealsTaken = posDeals;
        negativeIncrements = negIncs;
        negativeDealsTaken = negDeals;
    }
}

public class StatModManager : MonoBehaviour
{
    public static StatModManager Instance;

    // stat modifiers
    public Dictionary<StatName, StatDescription> StatModifiers = new Dictionary<StatName, StatDescription>
    {
        { StatName.MOVEMENT_SPEED,         new StatDescription(1.05f, 0, null, 0)},
        { StatName.DAMAGE_OUTPUT,          new StatDescription(1.35f, 0, null, 0) },
        { StatName.FIRE_SPEED,             new StatDescription(1.2f,  0, null, 0) },
        { StatName.RELOAD_SPEED,           new StatDescription(1.2f,  0, new List<float>{0.9f, 0.7f, 0.3f}, 0) },
        { StatName.MAGAZINE_SIZE,          new StatDescription(2,     0, null, 0) },
        { StatName.JUMP_HEIGHT,            new StatDescription(1.2f,  0, null, 0) },
        { StatName.SLIDE_DISTANCE,         new StatDescription(1,     0, new List<float>{0.75f, 0.5f, 0f}, 0) },
        { StatName.HEADSHOT_BONUS,         new StatDescription(0.2f,  0, new List<float>{0.75f, 0.5f, 0f}, 0) },
        { StatName.LADY_PROJECTILE_SPEED,  new StatDescription(1,     0, new List<float>{1.05f, 1.15f, 1.35f}, 0) },
        { StatName.DOG_RECOVERY_SPEED,     new StatDescription(1,     0, new List<float>{1.1f, 1.25f, 1.5f}, 0) },
    };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // add a stat modifier, can be timed but idk if that would be used for stats
    public void AddStatModifier (StatName statName, DealType dealType, float timerLength = -1f, System.Func<bool> timerEndFunction = null)
    {
        // permanently add stat modifier
        if (timerLength == -1f || timerEndFunction == null)
        {
            if (dealType == DealType.POSITIVE)
                StatModifiers[statName].positiveDealsTaken += 1;
            else
                StatModifiers[statName].positiveDealsTaken -= 1;
        }
        // temporarily add stat modifier (must have both length and end function) 
        else
        {
            if (dealType == DealType.POSITIVE)
                StatModifiers[statName].positiveDealsTaken += 1;
            else
                StatModifiers[statName].positiveDealsTaken -= 1;
            TimerHandler.Instance.CreateTimerHandle(timerLength, timerEndFunction);
        }

        GameManager.Instance.SetStatMod(statName);
        
    }

    public float GetPositiveStatModifier (StatName statName)
    {
        if (statName == StatName.MAGAZINE_SIZE) // bc magazine size is additive
        {
            if (StatModifiers[statName].positiveDealsTaken == 0) return 0;
            return StatModifiers[statName].positiveIncrement * StatModifiers[statName].positiveDealsTaken;
        }
        if (StatModifiers[statName].positiveDealsTaken == 0) return 1f;
        return math.pow(StatModifiers[statName].positiveIncrement, StatModifiers[statName].positiveDealsTaken);
    }
    public float GetNegativeStatModifier (StatName statName)
    {
        if (StatModifiers[statName].negativeIncrements == null) return 1f;
        if (StatModifiers[statName].negativeDealsTaken == 0) return 1f;
        if (StatModifiers[statName].negativeDealsTaken == 4) { 
            GameManager.Instance.FourNegativeDeals(statName);
            return 0; 
        }
        return StatModifiers[statName].negativeIncrements[StatModifiers[statName].negativeDealsTaken - 1]; 
    }

}
