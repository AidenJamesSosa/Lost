using System;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [SerializeField] GameObject mTurnIndicator;

    public float CooldownDuration => 1f / Speed;
    public float CooldownTimeRemaining { get; private set; }

    public event Action OnTurnFinsihed;
    void Awake()
    {
        CooldownTimeRemaining = CooldownDuration;
    }
    public void TakeTurn()
    {
        Invoke("FinishTurn", 1);
        mTurnIndicator.SetActive(true);
        CooldownTimeRemaining = CooldownDuration;
    }
    public void FinishTurn()
    {
        mTurnIndicator.SetActive(false);
        OnTurnFinsihed? .Invoke();
    }
}
