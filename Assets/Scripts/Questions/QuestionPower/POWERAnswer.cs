using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class POWERAnswer : MonoBehaviour
{
    [SerializeField] private POWERManager powerManager;
    [Header("Components")]
    [SerializeField] private Image buttonBorder;
    [SerializeField] private Image powerImage;
    [SerializeField] private RectTransform rectGlowTransform;
    [SerializeField] private Image rectGlowImage;
    [SerializeField] private LoadingRectangle loadingRectangle;
    

    [Header("Attributes")]
    [SerializeField] private float powerOnClick;
    [SerializeField] private float powerDecay;
    [SerializeField] private float maxPowerTime;
    [SerializeField] private AnimationCurve powerColorCurve;
    
    private Color colorNoPower;
    private Color colorPower;
    private Vector2 defaultGlowSizeDelta;
    private bool powered;
    private float poweredTime;
    private float power;

    
    
    void Start()
    {
        defaultGlowSizeDelta = rectGlowTransform.sizeDelta;
        colorNoPower = powerImage.color;
        colorPower = GameManager.Instance.buttonGreen;
    }
    
    void Update()
    {
        if(!powerManager.powered)
        {
            if (!powered && power > 0)
            {
                power = Mathf.Clamp01(power - (powerDecay*Time.deltaTime));
                powerImage.color = Color.Lerp(colorNoPower, colorPower, powerColorCurve.Evaluate(power));
            }

            if (powered)
            {
                poweredTime += Time.deltaTime;
            
                float fillAmount = 1 - (poweredTime / maxPowerTime);
            
                loadingRectangle.UpdateLine(fillAmount);
            
            
            
                if (poweredTime >= maxPowerTime)
                {
                    PoweredEnd();
                }
            
            }
        }
    }

    public void OnClick()
    {
        if (!powered)
        {
            power = Mathf.Clamp01(power + powerOnClick);

            if (power >= 1)
            {
                PoweredStart();
            }   
        }
    }

    private void PoweredEnd()
    {
        GameManager.Instance.FadeImageColorInOut(GameManager.Instance.buttonRed, 0.1f, 0.5f, buttonBorder);
        loadingRectangle.End();
        powered = false;
        
        powerManager.OnPoweredEnd();
    }
    
    private void PoweredStart()
    {
        PowerAnimation(GameManager.Instance.buttonGreen, 0.6f, 0.35f, 0.05f, 2.0f);
        powered = true;
        poweredTime = 0.0f;

        powerManager.OnPoweredStart();
    }

    public void PowerAnimation(Color color, float sizeOutTime, float fadeOutTime, float waitFadeOutTime, float distance)
    {
        DOTween.Kill(rectGlowTransform);
        DOTween.Kill(rectGlowImage);

        rectGlowImage.color = color;
        
        rectGlowTransform.sizeDelta = defaultGlowSizeDelta;
        
        rectGlowTransform.DOSizeDelta(new Vector2(100*distance,100*distance), sizeOutTime).SetId("answerTween").SetEase(Ease.OutCubic);
        rectGlowImage.DOFade(1.0f,waitFadeOutTime).OnComplete(() => 
            rectGlowImage.DOFade(0.0f, fadeOutTime).SetId("answerTween").SetEase(Ease.InSine));
    }

    public void FadeOutProgress(float duration)
    {
        loadingRectangle.FadeOut(duration);
    }
}
