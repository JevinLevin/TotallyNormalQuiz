using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using System;
using Unity.VisualScripting;

public class SWITCHClickCheck : MonoBehaviour
{

    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private AnswerGeneric answerScript;
    [SerializeField] private SWITCHRandomiseAnswers answerRandomiser;

    [Header("Answer")]
    [SerializeField] public bool correctAnswer;
    [SerializeField] public TextMeshProUGUI answerText;
    private bool mouseOver;
    private bool hasClicked;
    
    void Update()
    {


        if (mouseOver && answerRandomiser.clicked)
        {
            if(hasClicked)
            {
                if(correctAnswer)
                {
                    answerRandomiser.RandomiseAnswers();
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                ClickAnswer();
            }
        }

        hasClicked = false;
        hasClicked = Input.GetMouseButtonDown(0); 
    }

    public void SetCorrect(bool value)
    {
        answerScript.SetCorrect(value);
    }

    public void OnMouseEnter()
    {
        mouseOver = true;
    }

    public void OnMouseExit()
    {
        mouseOver = false;
    }

    private void ClickAnswer()
    {
        if(correctAnswer)
        {
            questionScript.ClickAnswerGeneric(answerScript, true);
        }
        else
        {
            questionScript.ClickAnswerGeneric(answerScript, false);
        }
    }

}
