using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class RessourcesLife : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image imageFillAmount;


        private void Start()
        {
            image.DOFade(0, 0);
            imageFillAmount.DOFade(0, 0);
        }
        public void SetRessourcesUi(Ressource ressource)
        {
            image.DOFade(1, 0);
            imageFillAmount.DOFade(1, 0);
            imageFillAmount.DOFillAmount(ressource.lifePoint / ressource.startLifePoint, 0.1f);
            
            image.DOFade(0, 5).SetEase(Ease.InOutExpo);
            imageFillAmount.DOFade(0, 5).SetEase(Ease.InOutExpo);
        }

        public IEnumerator Hide(float time)
        {
            yield return new WaitForSeconds(0.1f);
            image.DOKill();
            imageFillAmount.DOKill();
            image.DOFade(0, time);
            imageFillAmount.DOFade(0, time);
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }

    }
