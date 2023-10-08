using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ORDERManager : MonoBehaviour
{
    [SerializeField] private QuestionGenericMulti questionMultiScript;

    public void SetCorrect(MultiAnswer[] answers)
    {

        // Creates a copy and reshuffles in order
        MultiAnswer[] tempAnswers = new MultiAnswer[answers.Length];
        Array.Copy(answers, tempAnswers, answers.Length);
        tempAnswers = tempAnswers.OrderBy( x => x.originalIndex ).ToArray();

        string correctAnswer = "";

        switch (questionMultiScript.currentPhaseNumber)
        {
            
            case 0:
                correctAnswer = tempAnswers[0].answer;
                questionMultiScript.SetQuestionTitle("Click the 1st answer");
                break;
            case 1:
                correctAnswer = tempAnswers[1].answer;
                questionMultiScript.SetQuestionTitle("Click the 2nd answer");
                break;
            case 2:
                correctAnswer = tempAnswers[2].answer;
                questionMultiScript.SetQuestionTitle("Click the 3rd answer");
                break;
            case 3:
                correctAnswer = tempAnswers[3].answer;
                questionMultiScript.SetQuestionTitle("Click the 4th answer");
                break;
        }

        foreach( MultiAnswer answer in answers)
        {
            if (answer.answer == correctAnswer)
            {
                answer.correct = true;
            }
        }

    }
}
