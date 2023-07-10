using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorShop : Shop
{

    [SerializeField]Transform Door;
    protected override void Buy()
    {
        Door.DOMoveY(Door.position.y - 10,1);
        base.Buy();
    }
}
