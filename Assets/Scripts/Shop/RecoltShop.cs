using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RecoltShop : Shop
{

    [SerializeField] private Object RefToSpawn;
    [SerializeField] private Transform positionToSpawn;


    protected override void Buy()
    {
        if (!closed)
        {
            GameObject obj =(GameObject)Instantiate(RefToSpawn, positionToSpawn.position, Quaternion.identity);
            StartCoroutine(ActiveAI(obj.GetComponent<AICharacter>()));
            base.Buy();
        }
    }

    IEnumerator ActiveAI(AICharacter Character)
    {
        yield return new WaitForEndOfFrame();
        Character.isActive = true;

    }
}
