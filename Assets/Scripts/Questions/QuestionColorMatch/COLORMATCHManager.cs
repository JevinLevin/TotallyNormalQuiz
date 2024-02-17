using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class COLORMATCHManager : MonoBehaviour
{
    private QuestionMultiGeneric questionMultiScript;
    [SerializeField] private TextMeshProUGUI phaseText;
    //private GameManager.GameColors[] possibleColors;

    private COLORMATCHAnswer[] answers;

    private void Awake()
    {
        questionMultiScript = GetComponent<QuestionMultiGeneric>();

        answers = GetComponentsInChildren<COLORMATCHAnswer>();
    }

    private void OnEnable()
    {
        questionMultiScript.OnSetAnswers += SetCorrect;
        questionMultiScript.OnSetQuestion += SetColours;
        questionMultiScript.OnWin += StopShift;
    }

    private void SetCorrect(MultiAnswer[] answers)
    {
        // Chose random answer
        MultiAnswer correctAnswer = answers[Random.Range(0, answers.Length)];
        correctAnswer.correct = true;
        phaseText.text = correctAnswer.answer;
    }

    private void SetColours()
    {
        // Reset colors
        foreach (AnswerGeneric answer in questionMultiScript.Answers)
            answer.SetDefaultColors();
        
        int currentPhase = questionMultiScript.CurrentPhase;
        if (currentPhase == 0)
            return;
        
        List<GameManager.GameColors> possibleColors;
        if (currentPhase is (>= 1 and <= 2) or >= 5)
        {
            possibleColors = GetPossibleColors(currentPhase).ToList();
            // If it breaks the loop then its all good
            for (int i = 0; i < questionMultiScript.Answers.Count; i++)
            {
                questionMultiScript.Answers[i].SetTextColor(GameManager.ColorDictionary[possibleColors[i]]);
            }
        }
        if (currentPhase >= 3)
        {
            possibleColors = GetPossibleColors(currentPhase).ToList();
            // If it breaks the loop then its all good
            for (int i = 0; i < questionMultiScript.Answers.Count; i++)
            {
                questionMultiScript.Answers[i].frontImage.color = GameManager.ColorDictionary[possibleColors[i]];
            }
        }

        if (currentPhase >= 7)
        {
            foreach (COLORMATCHAnswer answerScript in answers)
            {
                answerScript.isShifting = true;
                if(currentPhase == 7)
                    answerScript.hueSpeed = 3;
                if(currentPhase == 8)
                    answerScript.hueSpeed = 2;
                if(currentPhase == 9)
                    answerScript.hueSpeed = 1f;
            }
        }
    }

    private void StopShift()
    {
        foreach (COLORMATCHAnswer answerScript in answers)
        {
            answerScript.Reset();
        }
    }

    private GameManager.GameColors[] GetPossibleColors(int currentPhase = 0)
    {
        bool valid = false;
        int limit = 100;

        GameManager.GameColors[] possibleColors = new GameManager.GameColors[questionMultiScript.Answers.Count];

        while (!valid && limit > 0)
        {
            limit--;
            
            if (currentPhase >= 1)
                possibleColors = ((GameManager.GameColors[])Enum.GetValues(typeof(GameManager.GameColors))).Take(4).ToArray();
            if (currentPhase >= 5)
                possibleColors = (GameManager.GameColors[])Enum.GetValues(typeof(GameManager.GameColors));

            possibleColors.Shuffle();
            
            valid = true;
            
            for (int i = 0; i < questionMultiScript.Answers.Count; i++)
            {
                AnswerGeneric answer = questionMultiScript.Answers[i];
                GameManager.GameColors color = possibleColors[i];
                
                // If the color matches the text
                if (answer.GetText() == color.ToString())
                    valid = false;
        
                // If the text color is already the same
                if (answer.GetTextColor() == GameManager.ColorDictionary[color])
                    valid = false;
                
                // If the front color is already the same
                if (answer.frontImage.color == GameManager.ColorDictionary[color])
                    valid = false;
            }

        }

        return possibleColors;
    }
}
