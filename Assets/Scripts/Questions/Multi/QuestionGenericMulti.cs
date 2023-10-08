using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class QuestionGenericMulti : MonoBehaviour
{

    public int currentPhaseNumber;

    [SerializeField] private List<MultiPhaseScriptableObject> phases;
    [SerializeField] private QuestionGeneric questionScript;

    [Header("Events")]
    public UnityEvent OnWin;
    public UnityEvent OnFail;
    public UnityEvent OnCorrect;
    public UnityEvent OnReset;
    public UnityEvent OnStart;
    public UnityEvent<MultiAnswer[]> OnSetQuestion;

    private int previousIndex = -1;


    public void QuestionFail()
    {
        FadeAnswers(GameManager.Instance.buttonRed);

        questionScript.GenericAnswerWrong();

        OnFail?.Invoke();
    }

    private void QuestionWin()
    {
        FlashAnswers(GameManager.Instance.buttonGreen);

    
        questionScript.SetQuestionTitle("VICTORY!!!");
        FadeAnswerText();
        
        questionScript.GenericAnswerCorrect();        
    }

    public void QuestionStart()
    {
        OnStart?.Invoke();
    }

    public void QuestionReset()
    {
        currentPhaseNumber = 0;

        DisplayQuestion();

        OnReset?.Invoke();
    }

    public void ClickAnswer(AnswerGeneric answer)
    {
        if(answer.GetCorrect()) CorrectAnswer();
        else WrongAnswer();
    }

    private void CorrectAnswer()
    {
        //answer.SetDefaultColors();
        currentPhaseNumber++;

        if(currentPhaseNumber >= phases.Count)
        {
            QuestionWin();
            OnWin?.Invoke();
            //CountdownWin();
        }
        else
        {

            DisplayQuestion();

            OnCorrect?.Invoke();

            FadeAnswersInOut(GameManager.Instance.buttonGreen);
        }
    }

    private void WrongAnswer()
    {
        QuestionFail();
        OnFail?.Invoke();
        //CountdownFail();
    }

    public void FadeAnswersInOut(Color fadeColor)
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
            DOTween.Kill(answer.backImage);
            GameManager.Instance.FadeImageColorInOut(fadeColor, 0.25f, 0.5f, answer.backImage);
        }
    }

    public void FadeAnswers(Color fadeColor)
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
            DOTween.Kill(answer.backImage);
            GameManager.Instance.FadeImageColor(fadeColor, 0.15f, answer.backImage);
        }
    }

    public void FadeAnswerText()
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
           answer.FadeText(0.25f);
        }
    }

    public void FlashAnswers(Color fadeColor)
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
            GameManager.Instance.FlashImageColor(fadeColor, 0.25f, answer.backImage);
        }
    }

    public void DisplayQuestion()
    {
        MultiPhaseScriptableObject currentPhase = phases[currentPhaseNumber];
        currentPhase.SetupIndex();

        List<MultiAnswer> allAnswers = currentPhase.answers.ConvertAll(MultiAnswer => new MultiAnswer(MultiAnswer.answer, MultiAnswer.correct, MultiAnswer.originalIndex));

        List<MultiAnswer> correctAnswers = new List<MultiAnswer>();
        List<MultiAnswer> wrongAnswers = new List<MultiAnswer>();

        foreach(MultiAnswer answer in allAnswers)
        {
            if(answer.correct)
            {
                correctAnswers.Add(answer);
            }
            else
            {
                wrongAnswers.Add(answer);
            }
        }

        correctAnswers.Shuffle();
        wrongAnswers.Shuffle();

        MultiAnswer[] newAnswers = new MultiAnswer[questionScript.answers.Count];

        if(correctAnswers.Count > 0) 
        {
            newAnswers[0] = correctAnswers[0];
            for(int i = 1; i < newAnswers.Length; i++)
            {
                newAnswers[i] = wrongAnswers[i-1];
            }
        }
        else
        {
            for(int i = 0; i < newAnswers.Length; i++)
            {
                newAnswers[i] = wrongAnswers[i];
            }
        }

        newAnswers.Shuffle();

        OnSetQuestion?.Invoke(newAnswers);

        int correctIndex = 0;
        do
        {
            for(int i = 0; i < newAnswers.Length; i++)
            {
                questionScript.answers[i].SetText(newAnswers[i].answer);
                questionScript.answers[i].SetCorrect(newAnswers[i].correct);
                if(newAnswers[i].correct)
                {
                    correctIndex = i;
                }
            }

            // Has to reshuffle just incase it loops
            newAnswers.Shuffle();
        }
        // Makes sure the player cant click the same answer to win
        while(currentPhaseNumber == phases.Count-2 && correctIndex == previousIndex);

        previousIndex = correctIndex;
    }

    public int GetPhaseCount()
    {
        return phases.Count;
    }

    public void SetQuestionTitle(string title)
    {
        questionScript.SetQuestionTitle(title);
    }
}
