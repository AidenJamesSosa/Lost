using System.Collections.Generic;
using UnityEngine;

public class BattlePartyComponent : MonoBehaviour
{
    [SerializeField] BattleCharacter[] mBattleCharactersPrefab;

    List<BattleCharacter> mBattleCharacters;
    IViewClient mOwnerViewClient;
    void Awake()
    {
        mOwnerViewClient = GetComponent<IViewClient>();
    }
    public void FinishPrep()
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
                mBattleCharacters.Add(Instantiate(battleCharacter));
            }
        }
        return mBattleCharacters;
    }
}
