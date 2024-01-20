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
    private GameManager.GameColors[] possibleColors;

    private void Awake()
    {
        questionMultiScript = GetComponent<QuestionMultiGeneric>();
    }

    private void OnEnable()
    {
        questionMultiScript.OnSetAnswers += SetCorrect;
        questionMultiScript.OnSetQuestion += SetColours;
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

        if (currentPhase == 1)
            possibleColors = ((GameManager.GameColors[])System.Enum.GetValues(typeof(GameManager.GameColors))).Take(5).ToArray();
        if (currentPhase == 5)
            possibleColors = (GameManager.GameColors[])System.Enum.GetValues(typeof(GameManager.GameColors));


        int count = 0;
        foreach (AnswerGeneric answer in questionMultiScript.Answers)
        {
            GameManager.GameColors randomColor = SetRandomColor(answer, count, getAnswer => getAnswer.GetTextColor());
            answer.SetTextColor(GameManager.ColorDictionary[randomColor]);

            if (currentPhase >= 3)
            {
                GameManager.GameColors randomColor2 = SetRandomColor(answer, count,  getAnswer => getAnswer.frontImage.color);
                answer.frontImage.color = GameManager.ColorDictionary[randomColor2];
            }

            count++;
        }
    }

    private GameManager.GameColors SetRandomColor(AnswerGeneric answer, int count, Func<AnswerGeneric, Color> getColor)
    {
        GameManager.GameColors randomColor;
        int failSafe = 0;
        do
        {
            randomColor = GetRandomColor();
            failSafe++;
        }
        while ( failSafe < 25 && CheckDuplicateColors(answer, randomColor, count, getColor));
        return randomColor;
    }

    private bool CheckDuplicateColors(AnswerGeneric answer, GameManager.GameColors color, int index, Func<AnswerGeneric, Color> CheckColor)
    {
        // If the color matches the text
        if (answer.GetText() == color.ToString())
            return true;
        
        // If the text color is already the same
        if (questionMultiScript.Answers[index].GetTextColor() == GameManager.ColorDictionary[color])
            return true;


        // If the color has already been used
        if (questionMultiScript.Answers.Any( currentAnswer => CheckColor?.Invoke(currentAnswer) == GameManager.ColorDictionary[color]))
            return true;
        
        return false;
    }
    
    private GameManager.GameColors GetRandomColor()
    {
        return possibleColors[Random.Range(0, possibleColors.Length)];
    }
}
