using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BattlePartyComponent : MonoBehaviour
{
    [SerializeField] BattleCharacter[] mBattleCharactersPrefab;

    List<BattleCharacter> mBattleCharacters;
    IViewClient mOwnerViewClient;
    public event Action<BattleCharacter> mBattleCharacterTakeTurn;
    void Awake()
    {
        mOwnerViewClient = GetComponent<IViewClient>();
    }
    public void FinishPrep()
    {
    }
    public void UpdateView()
    {
        if (mOwnerViewClient is not null)
        {
            mOwnerViewClient.SetViewTarget(mBattleCharacters[0].transform);
            mOwnerViewClient.ResetViewAngle();
        }
    }
    public List<BattleCharacter> GetBattleCharacters()
    {
        if (mBattleCharacters == null)
        {
            mBattleCharacters = new List<BattleCharacter>();
            foreach (BattleCharacter battleCharacter in mBattleCharactersPrefab)
            {
                //BattleCharacter newBattleCharacter = Something
                BattleCharacter newBattleCharacter = Instantiate(battleCharacter);
                newBattleCharacter.onTurnStarted += CharacterInTurn;
                mBattleCharacters.Add(newBattleCharacter);
            }
        }
        return mBattleCharacters;
    }
    private void CharacterInTurn(BattleCharacter character)
    {
        mBattleCharacterTakeTurn?.Invoke(character);
        if (mOwnerViewClient is not null && character)
        {
            mOwnerViewClient.SetViewTarget(character.transform);
        }
    }
}
