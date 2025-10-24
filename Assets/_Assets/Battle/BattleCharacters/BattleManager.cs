using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BattleManager : MonoBehaviour
{
    List<BattleSite> mBattleSites;
    List<BattleCharacter> mBattleCharacters = new List<BattleCharacter>();
    Queue<BattleCharacter> mFirstRoundBattleCharacters = new Queue<BattleCharacter>();
    //int Roundnumber = 1;
    //int mFirstTurnNextIndex = 0;
    public void StartBattle(BattlePartyComponent playerParty, BattlePartyComponent enemyParty)
    {
        mBattleCharacters.Clear();
        if (mBattleSites == null)
        {
            mBattleSites = new List<BattleSite>();
            mBattleSites.AddRange(GameObject.FindObjectsByType<BattleSite>(FindObjectsSortMode.None));
        }
        //Debug.Log($"Staring Battle between: {playerParty.gameObject.name} and {enemyParty.gameObject.name}");
        PrepParty(playerParty);
        PrepParty(enemyParty);
        StartCoroutine(StartTurns());
    }
    IEnumerator StartTurns()
    {//Todo
        yield return new WaitForSeconds(2);
        UpdateTurnOrder();
        Debug.Log("Sarting first turn");
        mFirstRoundBattleCharacters = new Queue<BattleCharacter>(mBattleCharacters);
        ProcessFirstRound();
    }
    private void ProcessFirstRound()
    {
        if (mFirstRoundBattleCharacters.TryDequeue(out BattleCharacter nextBattleCharacter))
        {
            if (mBattleCharacters.Contains(nextBattleCharacter))
            {
                nextBattleCharacter.TakeTurn();
            }
            else
            {
                ProcessFirstRound();
            }
            return;
        }
        
        foreach(BattleCharacter battleCharacter in mBattleCharacters)
        {
            battleCharacter.OnTurnFinsihed -= ProcessFirstRound;
            battleCharacter.OnTurnFinsihed += NextTurn;
        }
        NextTurn();
    }
    private void NextTurn()
    {
        UpdateTurnOrder();
        Debug.Log("NextTurn");
        float advanceTime = mBattleCharacters[0].CooldownTimeRemaining;
        foreach (BattleCharacter battleCharacter in mBattleCharacters)
        {
            battleCharacter.AdvanceCooldown(advanceTime);
        }
        BattleCharacter nextInTurn = mBattleCharacters[0];
        nextInTurn.TakeTurn();

        mBattleCharacters.Remove(nextInTurn);
        mBattleCharacters.Add(nextInTurn);

        //mBattleCharacters
    }
    
    private void UpdateTurnOrder()
    {
        mBattleCharacters = mBattleCharacters.OrderBy((battleCharacter) => { return battleCharacter.CooldownTimeRemaining; }).
        ThenBy((battleCharacter) => { return 1/battleCharacter.Speed; }).
        ToList();
    }
    private void PrepParty(BattlePartyComponent party)
    {

        BattleSite partyBattleSite = mBattleSites.Find((BattleSite) => { return !BattleSite.IsPlayerSite; });
        if (party.gameObject.CompareTag("Player"))
        {
            partyBattleSite = mBattleSites.Find((BattleSite) => { return BattleSite.IsPlayerSite; });
        }
        int i = 0;
        foreach (BattleCharacter partyBattleCharacter in party.GetBattleCharacters())
        {
            partyBattleCharacter.transform.position = partyBattleSite.GetPositionForUnit(i);
            partyBattleCharacter.transform.rotation = partyBattleSite.transform.rotation;
            partyBattleCharacter.OnTurnFinsihed += ProcessFirstRound;
            mBattleCharacters.Add(partyBattleCharacter);
            i++;
        }
        party.UpdateView();
    }
}
