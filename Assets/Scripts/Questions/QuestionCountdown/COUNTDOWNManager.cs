using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private QuestionGenericMulti questionMultiScript;
    [SerializeField] private TextMeshProUGUI phaseNumberDisplay;
    [SerializeField] private Image countdownBar;
    [Title("Attributes")]
    [SerializeField]
    private float countdownMax;
    [SerializeField]
    private float correctBonus;

    void Awake()
    {
        defaultCountdownBarColor = countdownBar.color;
    }

    void Update()
    {
        if(countdown)
        {
            SetCountdownBar();
            countdownTimer += Time.deltaTime;
            if(countdownTimer > countdownMax)
            {
                questionMultiScript.QuestionFail();
            }
        }
    }

    public void SetPhaseNumber()
    {
        phaseNumberDisplay.text = questionMultiScript.currentPhaseNumber.ToString();
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
        GameManager.Instance.FlashImageColor(GameManager.Instance.buttonGreen, 0.25f, countdownBar);
        phaseNumberDisplay.text = "-";
    }

    public void CountdownFail()
    {
        CountdownEnd();

        GameManager.Instance.FadeImageColor(GameManager.Instance.buttonRed, 0.15f, countdownBar);
    }

    public void CountdownCorrect()
    {
        countdownTimer = Mathf.Clamp(countdownTimer - correctBonus, 0, countdownMax);
        GameManager.Instance.FadeImageColorInOut(GameManager.Instance.buttonGreen, 0.25f, 0.5f, countdownBar);
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
