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
    private int currentPhaseNumber;
    private Color defaultCountdownBarColor;

    [SerializeField] private List<COUNTDOWNPhaseScriptableObject> phases;
    [Title("Components")]
    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private Image countdownBar;
    [SerializeField] private TextMeshProUGUI phaseNumberDisplay;
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
                CountdownFail();
            }
        }
    }
   
    public void CountdownReset()
    {
        countdown = false;
        countdownTimer = 0.0f;
        currentPhaseNumber = phases.Count-1;

        DisplayQuestion();
        ResetCountdownBar();
    }

    public void CountdownStart()
    {
        countdown = true;
    }

    private void CountdownEnd()
    {
        countdown = false;
        
    }

    private void CountdownWin()
    {
        FlashAnswers(GameManager.Instance.buttonGreen);

        phaseNumberDisplay.text = "-";
        questionScript.SetQuestionTitle("VICTORY!!!");
        FadeAnswerText();
        
        questionScript.GenericAnswerCorrect();
    }

    private void CountdownFail()
    {
        CountdownEnd();

        FadeAnswers(GameManager.Instance.buttonRed);

        questionScript.GenericAnswerWrong();
    }

    public void ClickAnswer(AnswerGeneric answer)
    {
        if(answer.GetCorrect()) CorrectAnswer();
        else WrongAnswer();
    }

    private void CorrectAnswer()
    {
        //answer.SetDefaultColors();
        currentPhaseNumber--;

        if(currentPhaseNumber < 0)
        {
            CountdownWin();
        }
        else
        {
            DisplayQuestion();
            countdownTimer = Mathf.Clamp(countdownTimer - correctBonus, 0, countdownMax);
            FadeAnswersInOut(GameManager.Instance.buttonGreen);
        }
    }

    private void WrongAnswer()
    {
        CountdownFail();
    }

    private void FadeAnswersInOut(Color fadeColor)
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
            GameManager.Instance.FadeImageColorInOut(fadeColor, 0.25f, 0.5f, answer.backImage);
        }
        GameManager.Instance.FadeImageColorInOut(fadeColor, 0.25f, 0.5f, countdownBar);
    }

    private void FadeAnswers(Color fadeColor)
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
            GameManager.Instance.FadeImageColor(fadeColor, 0.15f, answer.backImage);
        }
        GameManager.Instance.FadeImageColor(fadeColor, 0.15f, countdownBar);
    }

    private void FadeAnswerText()
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
           answer.FadeText(0.25f);
        }
    }

    private void FlashAnswers(Color fadeColor)
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
            GameManager.Instance.FlashImageColor(fadeColor, 0.25f, answer.backImage);
        }
        GameManager.Instance.FlashImageColor(fadeColor, 0.25f, countdownBar);
    }

    private void DisplayQuestion()
    {
        COUNTDOWNPhaseScriptableObject currentPhase = phases[currentPhaseNumber];
        List<COUNTDOWNAnswer> tempAnswers = currentPhase.answers;
        tempAnswers.Shuffle();
        phaseNumberDisplay.text = currentPhaseNumber.ToString();

        for(int i = 0; i < questionScript.answers.Count; i++)
        {
            questionScript.answers[i].SetText(tempAnswers[i].answer);
            questionScript.answers[i].SetCorrect(tempAnswers[i].correct);
        }
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
