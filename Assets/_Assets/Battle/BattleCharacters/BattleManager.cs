using System;
using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    List<BattleSite> mBattleSites;
    public void StartBattle(BattlePartyComponent playerParty, BattlePartyComponent enemyParty)
    {
        if (mBattleSites == null)
        {
            mBattleSites = new List<BattleSite>();
            mBattleSites.AddRange(GameObject.FindObjectsByType<BattleSite>(FindObjectsSortMode.None));
        }
        //Debug.Log($"Staring Battle between: {playerParty.gameObject.name} and {enemyParty.gameObject.name}");
        PrepParty(playerParty);
        PrepParty(enemyParty);
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
            i++;
        }
        party.FinishPrep();
    }
}
