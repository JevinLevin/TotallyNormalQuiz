using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionMultiGeneric : QuestionGeneric
{
    [SerializeField] private MultiPhase[] phases;
    public Action OnCorrect;
    public Action<MultiAnswer[]> OnNewQuestion;

    public int CurrentPhase { get; private set; }
    private int previousIndex;
    
    protected override void Reset()
    {
        base.Reset();
        
        CurrentPhase = 0;
        SetQuestion();
    }

    private void SetQuestion()
    {
        MultiPhase phase = phases[CurrentPhase];
        
        List<MultiAnswer> allAnswers = phase.answers.ConvertAll(multiAnswer => new MultiAnswer(multiAnswer.answer, multiAnswer.correct));
        
        List<MultiAnswer> correctAnswers = allAnswers.Where(answer => answer.correct).ToList();
        correctAnswers.Shuffle();
        List<MultiAnswer> wrongAnswers = allAnswers.Where(answer => !answer.correct).ToList();
        wrongAnswers.Shuffle();
        
        
        MultiAnswer[] newAnswers = new MultiAnswer[Answers.Count];
        int limit = Answers.Count;
        // At the end of the list, add a correct answer
        if (correctAnswers.Count > 0)
        {
            newAnswers[^1] = correctAnswers[0];
            limit--;
        }
        // Add wrong answers to the rest
        for (int i = 0; i < limit; i++)
            newAnswers[i] = wrongAnswers[i];
        newAnswers.Shuffle();
        
        OnNewQuestion?.Invoke(newAnswers);

        int correctIndex = 0;
        do
        {
            for(int i = 0; i < newAnswers.Length; i++)
            {
                Answers[i].SetText(newAnswers[i].answer);
                Answers[i].SetCorrect(newAnswers[i].correct);
                if(newAnswers[i].correct)
                {
                    correctIndex = i;
                }
            }

            // Has to reshuffle just incase it loops
            newAnswers.Shuffle();
        }
        // Makes sure the player cant click the same answer to win
        // Repeat shuffle the current question is two away from the end, and is in the same position as the last question
        while(CurrentPhase == phases.Length-2 && correctIndex == previousIndex);

        previousIndex = correctIndex;
        

    }

    private void QuestionMultiWin()
    {
        SetQuestionTitle("VICTORY!!!");

        FlashAnswersBack(GameManager.FlashImageColor, GameManager.ButtonGreen, 0.25f);
        
        GenericAnswerCorrect();
    }

    public void OnClickAnswer(AnswerGeneric answer)
    {
        if(answer.GetCorrect()) QuestionMultiCorrect();
        else QuestionMultiWrong();
    }

    public void QuestionMultiCorrect()
    {
        CurrentPhase++;

        // End questions once it reaches the end
        if (CurrentPhase >= phases.Length)
        {
            QuestionMultiWin();
            return;
        }

        FlashAnswersBack(GameManager.FadeImageColorInOut, GameManager.ButtonGreen, 0.25f,0.5f );

        SetQuestion();
        
        OnCorrect?.Invoke();
    }

    public void QuestionMultiWrong()
    {
        GenericAnswerWrong(true);
    }

    public int GetPhaseCount()
    {
        return phases.Length;
    }
}
