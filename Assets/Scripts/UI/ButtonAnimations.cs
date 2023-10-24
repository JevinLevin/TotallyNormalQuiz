using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public class ButtonAnimations : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected Image buttonImage;
    [SerializeField] protected Button button;

    [Header("Options")] 
    [SerializeField] protected bool disableColor;
    [SerializeField] protected bool disableScale;
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
        DOTween.Kill(rectTransform);
    }

    public void OnHover()
    {
        if(!GameManager.Instance.restarting)
        {
            if(!disableColor)
            {
                DOTween.Kill(buttonImage);
                buttonImage.DOColor(hoverButtonColor, 0.1f).SetEase(Ease.OutSine).SetId("hoverTween");
            }

            if (!disableScale)
            {
                DOTween.Kill(rectTransform);
                rectTransform.DOScale(1.03f, 0.1f);
            }
        }
        
    }

    public void OnHoverExit()
    {

        if(!GameManager.Instance.restarting)
        {
            if (!disableColor)
            {
                buttonImage.DOColor(buttonColor, 0.4f).SetEase(Ease.InSine).SetId("hoverTween");
            }

            if (!disableScale)
            {
                rectTransform.DOScale(1f, 0.2f);
            }
        }
        
    }


}
