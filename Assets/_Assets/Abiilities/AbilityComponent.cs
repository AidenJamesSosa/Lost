using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] initialAbilities;
    List<Ability> mabilities = new List<Ability> ();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Ability initialAbility in initialAbilities)
        {
            GiveAbility(initialAbility);
        }
    }
    private void GiveAbility(Ability abilityDefaultObject)
    {
        Ability newAbility = Instantiate(abilityDefaultObject);
        newAbility.Init(this);
        mabilities.Add(newAbility);
    }
    internal IEnumerable<Ability> GetAbilities()
    {
        return mabilities;
    }
}
