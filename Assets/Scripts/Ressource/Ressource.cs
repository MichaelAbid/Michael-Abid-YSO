using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum ERessourceType
{
    ItemRessources,
    HumanRessources
}

public class Ressource : MonoBehaviour
{
    [SerializeField] private ERessourceType ressource_type;

    [SerializeField] private bool infiniteRessource = false;
    [SerializeField] public float startLifePoint = 5;
    public float lifePoint = 5;
    [SerializeField] public int RessourceOnBreak = 5;
    [SerializeField] private ParticleSystem ps;

    [SerializeField] private Transform mesh;
    [SerializeField] private Collider col;
    [SerializeField] private RessourcesLife ressourcesLifeUI;
    private void Start()
    {
        lifePoint = startLifePoint;
    }

       private void OnTriggerEnter(Collider other)
        {
                Character chara = other.GetComponent<Character>();
                if (chara != null)
                {

                    chara.nearRessources.Add(this);

                }
        }

    private void OnTriggerExit(Collider other)
    {
        Character chara = other.GetComponent<Character>();
            if (chara != null)
            {
                chara.nearRessources.Remove(this);
            }
        }

    public void RetrieveRessources(Character character)
    {
        if (lifePoint <= 0) return;
        
        
        lifePoint -= character.strength;
        
        StartCoroutine(DelayedRetrieve(character));
        

    }

    IEnumerator DelayedRetrieve(Character character)
    {
        
        transform.DOShakePosition(0.2f, character.strength * 0.2f).SetEase(Ease.InOutSine);
        ressourcesLifeUI.SetRessourcesUi(this);
        if (lifePoint <= 0)
        {
            GameManager.Instance.AddRessourceOfType(ressource_type, RessourceOnBreak);
            ps.maxParticles = RessourceOnBreak;
            ps.Play();
            if (infiniteRessource)
            {
                lifePoint = startLifePoint;
                yield return new WaitForSeconds(0.1f);
                ressourcesLifeUI.SetRessourcesUi(this);
            }
            else
            {
                StartCoroutine(ressourcesLifeUI.Hide(0.2f));
                character.nearRessources.Remove(this);
                mesh.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutElastic);
                yield return new WaitForSeconds(0.3f);
                col.enabled = false;
                yield return new WaitForSeconds(1);
                Destroy(gameObject);
            }
        }
    }
}
