using System;
using UnityEngine;

//[RequireComponent (typeofCompenent)]
public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [field: SerializeField] public string Name { get; private set; } = "battleCharacter";
    [SerializeField] GameObject mTurnIndicator;

    public float CooldownDuration => 1f / Speed;
    public float CooldownTimeRemaining { get; private set; }

    public event Action<BattleCharacter> onTurnStarted;
    public event Action OnTurnFinsihed;

    AbilityComponent mAbilityComponet;

    public AbilityComponent GetAbilityComponent()
    {
        return mAbilityComponet;
    }
    void Awake()
    {
        //Speed = Speed + UnityEngine.Random.Range(0f, 1f);
        CooldownTimeRemaining = CooldownDuration;
        mTurnIndicator.SetActive(false);
        mAbilityComponet = GetComponent<AbilityComponent>();
    }
    public void TakeTurn()
    {
        Invoke("FinishTurn", 1);
        mTurnIndicator.SetActive(true);
        onTurnStarted?.Invoke(this);
        CooldownTimeRemaining = CooldownDuration;
    }
    public void FinishTurn()
    {
        mTurnIndicator.SetActive(false);
        OnTurnFinsihed?.Invoke();
    }
    internal void AdvanceCooldown(float advanceTime)
    {
        CooldownTimeRemaining -= advanceTime;
    }
}
