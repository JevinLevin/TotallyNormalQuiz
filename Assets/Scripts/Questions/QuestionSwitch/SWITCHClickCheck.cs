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
        
        
        // If mouse hovered over the button and not first frame
        if (mouseOver && answerRandomiser.clicked)
        {
            // If player clicked last frame
            if(hasClicked)
            {
                if(correctAnswer)
                {
                    answerRandomiser.RandomiseAnswers();
                }
            }
            // If the player lets go on the correct button
            if(Input.GetMouseButtonUp(0) && !answerRandomiser.mouseSpeed)
            {
                ClickAnswer();
            }
        }

        // This ensures that it only checks if the mouse was clicked last frame and not during the current frame
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
