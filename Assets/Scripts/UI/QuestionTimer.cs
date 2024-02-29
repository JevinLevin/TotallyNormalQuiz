using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class QuestionTimer : MonoBehaviour
{
    [SerializeField] 
    private QuestionGeneric questionScript;
    
    
    private Image countdownBar;
    private bool countdown;
    private float countdownTimer;
    private Color defaultCountdownBarColor;

    [Header("Attributes")]
    [SerializeField]
    private float countdownMax = 30;
    [SerializeField]
    private float correctBonus = 5;

    private Tween winTween;

    private void Awake()
    {
        countdownBar = GetComponent<Image>();
        
        defaultCountdownBarColor = countdownBar.color;
    }
    
    private void OnEnable()
    {
        questionScript.OnReset += CountdownReset;
        questionScript.OnStart += CountdownStart;
        questionScript.OnFail += CountdownFail;
        questionScript.OnWin += CountdownWin;
    }
    
    private void OnDisable()
    {
        questionScript.OnReset -= CountdownReset;
        questionScript.OnStart -= CountdownStart;
        questionScript.OnFail -= CountdownFail;
        questionScript.OnWin -= CountdownWin;
    }
    
    void Update()
    {
        if(countdown)
        {
            SetCountdownBar();
            countdownTimer += Time.deltaTime;
            if(countdownTimer > countdownMax)
            {
                questionScript.GenericAnswerWrong(true);
            }
        }
    }

    private void OnDestroy()
    {
        winTween.Kill();
    }

    public void CountdownReset()
    {
        countdown = false;
        countdownTimer = 0.0f;
        ResetCountdownBar();
    }

    public void CountdownStart()
    {
        countdown = true;
    }

    public void CountdownEnd()
    {
        countdown = false;
        
    }

    public void CountdownWin()
    {
        CountdownEnd();
        
        DOTween.Kill(countdownBar);
        winTween = GameManager.FlashImageColor(GameManager.ButtonGreen, 0.25f, countdownBar);
    }

    public void CountdownFail()
    {
        CountdownEnd();

        DOTween.Kill(countdownBar);
        GameManager.FadeImageColor(GameManager.ButtonRed, 0.15f, countdownBar);
    }

    public void CountdownCorrect()
    {
        countdownTimer = Mathf.Clamp(countdownTimer - correctBonus, 0, countdownMax);
        GameManager.FadeImageColorInOut(GameManager.ButtonGreen, 0.25f, 0.5f, countdownBar);
        
    }

    private void SetCountdownBar()
    {
        countdownBar.fillAmount = (countdownMax - countdownTimer) / countdownMax;
    }

    private void ResetCountdownBar()
    {
        countdownBar.color = defaultCountdownBarColor;
        countdownBar.fillAmount = 1;
    }
}
