using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    List<BattleSite> mBattleSites;
    List<BattleCharacter> mBattleCharacters = new List<BattleCharacter>();
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
        NextTurn();
    }
    private void NextTurn()
    {
        mBattleCharacters = mBattleCharacters.OrderBy((battleCharacter) => { return battleCharacter.CooldownTimeRemaining; }).ToList();

        mBattleCharacters[0].TakeTurn();
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
            partyBattleCharacter.OnTurnFinsihed += NextTurn;
            mBattleCharacters.Add(partyBattleCharacter);
            i++;
        }
        party.UpdateView();
    }
}
