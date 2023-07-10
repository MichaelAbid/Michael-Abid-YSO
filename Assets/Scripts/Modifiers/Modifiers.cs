using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EModifierType
{
    MovementSpeed,
    AttackSpeedModifier,
    
}

public class Modifiers
{

    public EModifierType modifierType;
    public float modifierTime;
    public float modifierAmount;
    public bool infinite;

    public Modifiers(EModifierType modifierType, float modifierTime, bool infinite, float modifierAmount)
    {
        this.modifierType = modifierType;
        this.modifierTime = modifierTime;
        this.infinite = infinite;
        this.modifierAmount = modifierAmount;
    }
}
