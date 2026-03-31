using UnityEngine;

[System.Serializable]
public abstract class DrawCondition : ScriptableObject
{
    public abstract bool ConditionMet();
}