using System;
using TMPro;
using UnityEngine;

public class CharacterControlWidget : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mCharacterNameText;
    internal void SetBattleCharacter(BattleCharacter battleCharacter)
    {
        Debug.Log($"Setting Battle Character name to: {battleCharacter.Name}");
        mCharacterNameText.SetText(battleCharacter.Name);
    }
}
