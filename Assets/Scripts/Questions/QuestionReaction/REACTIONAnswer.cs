using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class REACTIONAnswer : MonoBehaviour
{
    private AnswerGeneric answerScript;
    public bool Activated { get; set; }

    private Color defaultColor;

    private float currentTime;
    private float maxTime;

    private Action onFail;

    private Tween currentTween;

    public bool Delayed;
    private float activateDelay;
    [SerializeField] private float activateDelayMax = 0.5f;

    private void Awake()
    {
        answerScript = GetComponent<AnswerGeneric>();

        defaultColor = answerScript.frontImage.color;
    }

    private void Update()
    {
        if (Activated && !Delayed)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime)
                Fail();
        }

        if (Delayed)
        {
            activateDelay += Time.deltaTime;
            
            //print(activateDelay/activateDelayMax);
            //print(Activated);
        
            if (activateDelay >= activateDelayMax)
                Stop();
        }
}

    public void ActivateAnswer(float fadeInTime, Action onFail)
    {
        Activated = true;
        
        currentTween.Kill();
        currentTween = GameManager.Instance.FadeImageColor(GameManager.Instance.buttonRed, fadeInTime, answerScript.frontImage).SetEase(Ease.Linear);

        Delayed = false;
        currentTime = 0.0f;
        maxTime = fadeInTime;

        this.onFail = onFail;
    }

    public void OnClick()
    {
        if (!Activated || Delayed) return;
        
        currentTween.Kill();
        currentTween = GameManager.Instance.FadeImageColor(defaultColor, 0.15f, answerScript.frontImage);

        GameManager.Instance.FadeImageColorInOut(GameManager.Instance.buttonGreen, 0.15f, 0.25f, answerScript.backImage);

        Delayed = true;
        activateDelay = 0.0f;

    }
    
    public void Stop()
    {
        Activated = false;
        Delayed = false;
        currentTween.Kill();
    }

    private void Fail()
    {
        Activated = false;
        onFail?.Invoke();
    }


}
