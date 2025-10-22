using System;
using UnityEngine;

public class Ability : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }
    AbilityComponent mOwningAbilityComponent;
    internal void Init(AbilityComponent newAbility)
    {
        mOwningAbilityComponent = newAbility;
    }
    public virtual void ActivateAbility()
    {
        Debug.Log($"EA Games");
    }
}
