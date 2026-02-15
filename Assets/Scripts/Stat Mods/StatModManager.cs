using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatName {
    MOVEMENT_SPEED,
    DAMAGE_OUTPUT,
    FIRE_SPEED,
    RELOAD_SPEED,
    MAGAZINE_SIZE,
    JUMP_HEIGHT,
    HEADSHOT_BONUS,
    ENEMY_SATURATION, // idk what this means lol
}

public class StatModManager : MonoBehaviour
{
    public static StatModManager Instance;

    // stat modifiers
    private Dictionary<StatName, List<float>> StatModifiers = new Dictionary<StatName, List<float>>
    {
        { StatName.MOVEMENT_SPEED, new List<float>() },
        { StatName.DAMAGE_OUTPUT, new List<float>() },
        { StatName.FIRE_SPEED, new List<float>() },
        { StatName.RELOAD_SPEED, new List<float>() },
        { StatName.MAGAZINE_SIZE, new List<float>() },
        { StatName.JUMP_HEIGHT, new List<float>() },
        { StatName.HEADSHOT_BONUS, new List<float>() },
        { StatName.ENEMY_SATURATION, new List<float>() },
    };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // add a stat modifier, can be timed but idk if that would be used for stats
    public void AddStatModifier (StatName statName, float modifier, float timerLength = -1f, System.Func<bool> timerEndFunction = null)
    {
        // permanently add stat modifier
        if (timerLength == -1f || timerEndFunction == null)
        {
            StatModifiers[statName].Add(modifier);
        }
        // temporarily add stat modifier (must have both length and end function) 
        else
        {
            StatModifiers[statName].Add(modifier);
            TimerHandler.Instance.CreateTimerHandle(timerLength, timerEndFunction);
        }

        PlayerScriptRefHolder.Instance.SetStatMod(statName);
        
    }

    // gets all the stat mods for a stat
    public List<float> GetStatModsRaw (StatName statName)
    {
        return StatModifiers[statName];
    }

    // gets the total stat mods, can be changed to multiplicative iyw
    // when used call like this:
    // jumpHeight + StatModManager.Instance.GetTotalStatMod(StatName.JUMP_HEIGHT)
    // add to all uses of variable
    public float GetStatSum (StatName statName)
    {
        return StatModifiers[statName].Sum(x => x); 
    }
    public float GetStatProduct (StatName statName)
    {
        return StatModifiers[statName].Sum(x => x); 
    }

}
