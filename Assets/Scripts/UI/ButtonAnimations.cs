using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimations : MonoBehaviour
{

    [SerializeField] protected Image buttonImage;
    [SerializeField] protected Button button;
    protected Color buttonColor;
    protected Color hoverButtonColor;

    public virtual void Start()
    {
        buttonColor = buttonImage.color;
        hoverButtonColor = GameManager.Instance.buttonHoverColor;
    }

    void OnDestroy()
    {
        DOTween.Kill(buttonImage);
    }

    public void OnHover()
    {
        if(!GameManager.Instance.restarting)
        {
            DOTween.Kill(buttonImage);
            buttonImage.DOColor(hoverButtonColor, 0.1f).SetEase(Ease.OutSine).SetId("hoverTween");
        }
        
    }

    public void OnHoverExit()
    {

        if(!GameManager.Instance.restarting)
        {
            buttonImage.DOColor(buttonColor, 0.4f).SetEase(Ease.InSine).SetId("hoverTween");
        }
        
    }


}
