using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class COUNTDOWNManager : MonoBehaviour
{

    private bool countdown;
    private float countdownTimer;
    private Color defaultCountdownBarColor;


    [Title("Components")]
    private QuestionMultiGeneric questionMultiScript;
    [SerializeField] private TextMeshProUGUI phaseNumberDisplay;
    [SerializeField] private Image countdownBar;
    [Title("Attributes")]
    [SerializeField]
    private float countdownMax;
    [SerializeField]
    private float correctBonus;

    void Awake()
    {
        questionMultiScript = GetComponent<QuestionMultiGeneric>();
        defaultCountdownBarColor = countdownBar.color;
    }

    private void OnEnable()
    {
        questionMultiScript.OnReset += CountdownReset;
        questionMultiScript.OnStart += CountdownStart;
        questionMultiScript.OnFail += CountdownFail;
        questionMultiScript.OnCorrect += CountdownCorrect;
        questionMultiScript.OnWin += CountdownWin;
    }

    void Update()
    {
        if(countdown)
        {
            SetCountdownBar();
            countdownTimer += Time.deltaTime;
            if(countdownTimer > countdownMax)
            {
                questionMultiScript.QuestionMultiWrong();
            }
        }
    }

    public void SetPhaseNumber()
    {
        phaseNumberDisplay.text = (questionMultiScript.GetPhaseCount() -1- questionMultiScript.CurrentPhase).ToString();
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
        DOTween.Kill(countdownBar);
        GameManager.FlashImageColor(GameManager.ButtonGreen, 0.25f, countdownBar);
        phaseNumberDisplay.text = "-";
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
        
        // Set question title to current number
        SetPhaseNumber();
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
