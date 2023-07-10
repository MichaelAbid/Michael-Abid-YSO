using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] public AnimationCurve priceCurve;
    public int currentPrice;
    public int amountGiven;
    public int currentBuy;
    [SerializeField] public int maxBuy = 10;

    public bool closed;

    [SerializeField] public TextMeshProUGUI counter;

    public ERessourceType ressourceToUse;

    private void Start()
    {
        currentPrice = Mathf.RoundToInt(priceCurve.Evaluate(currentBuy/maxBuy-1));
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

        currentBuy++;
        
        amountGiven = 0;
        if (currentBuy >= maxBuy)
        {
            counter.text = "Sold Out";
            closed = true;
        }
        else
        {
            currentPrice = Mathf.RoundToInt(priceCurve.Evaluate((currentBuy+1) / (maxBuy)));
            counter.text = amountGiven + " / " + currentPrice;
        }
    }

    private void getNextPrice()
    {
        
        
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
