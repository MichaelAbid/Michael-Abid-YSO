using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] protected AnimationCurve priceCurve;
    protected int currentPrice;
    protected int amountGiven;
    protected int currentBuy;
    [SerializeField] protected int maxBuy = 10;

    protected bool closed;

    [SerializeField]protected TextMeshProUGUI counter;

    public ERessourceType ressourceToUse;

    private void Start()
    {
        currentPrice = Mathf.RoundToInt(priceCurve.Evaluate(currentBuy));
        counter.text = amountGiven + " / " + currentPrice;
    }

    public void AddAmount(int amount)
    {
        if (!closed)
        {
            amountGiven += amount;
            GameManager.Instance.AddRessourceOfType(ressourceToUse, -amount);
            counter.text = amountGiven + " / " + currentPrice;
            if (amountGiven >= currentPrice)
            {
                Buy();
            }
        }
    }
        

    protected virtual void Buy()
    {
        
        getNextPrice();
        amountGiven = 0;
        counter.text = amountGiven + " / " + currentPrice;
        if (currentBuy > maxBuy)
        {
            counter.text = "Sold Out";
            closed = true;
        }
    }

    private void getNextPrice()
    {
        currentBuy++;
        currentPrice = Mathf.RoundToInt(priceCurve.Evaluate(currentBuy));
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
        if (pc != null)
        {
            pc.currentShop = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
        if (pc != null)
        {
            pc.currentShop = null;
        }
    }

}
