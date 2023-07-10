using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance => m_instance;
    [SerializeField] private Camera m_mainCamera;
    public static Camera mainCamera => Instance.m_mainCamera;
    [SerializeField] private Dictionary<ERessourceType, int> ressources = new Dictionary<ERessourceType, int>();
    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }



    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(ERessourceType)).Length; i++)
        {
            ressources[(ERessourceType)i] = 0;
        }
    }


   public void AddRessourceOfType(ERessourceType type, int amount)
    {
        ressources[type] += amount;
        texts[(int)type].text = ressources[type].ToString();
        texts[(int)type].transform.DOShakePosition(0.4f, Vector3.up * 2f);
        if (amount >= 0)
        {
            texts[(int)type].DOColor(Color.green,0.1f).OnComplete(()=>ReturnToBasEcolor((int)type));
        }
        else
        {
            texts[(int)type].DOColor(Color.red, 0.1f).OnComplete(() => ReturnToBasEcolor((int)type));
        }
    }
    public void ReturnToBasEcolor(int i)
    {
        texts[i].DOColor(Color.white, 0.1f);
    }
    public int GetRessourcesOfType(ERessourceType type)
    {
        return ressources[type];
    }

}
