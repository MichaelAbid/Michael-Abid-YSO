using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ETarget
{
    ALL,
    AI,
    Player
}

public class BuffShop : Shop
{

    [SerializeField]EModifierType modifierType;
    [SerializeField] float modifierTime;
    [SerializeField] bool infinite;
    [SerializeField] float modifierAmount;
    [SerializeField] ETarget target;
    protected override void Buy()
    {
        List<Character> list = new List<Character>();
        switch (target)
        {
            case ETarget.ALL:
                list = FindObjectsOfType<Character>().ToList();
                break;
            case ETarget.AI:
                list = FindObjectsOfType<AICharacter>().Cast<Character>().ToList();
                break;
            case ETarget.Player:
                list = FindObjectsOfType<PlayerCharacter>().Cast<Character>().ToList();
                break;
            default:
                break;
        }

        foreach (Character character in list)
        {
            character.modifiers.Add(new Modifiers(modifierType, modifierTime, infinite, modifierAmount));
        }
        base.Buy();
    }
}
