using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GUITARAnswer : MonoBehaviour
{
    [SerializeField] private AnswerGeneric answerScript;
    [SerializeField] private GUITARManager guitarManager;
    [SerializeField] private RectTransform inputSpawner;
    private Queue<GUITARInput> currentInputs;


    private void Awake()
    {
        currentInputs = new Queue<GUITARInput>();
    }
    

    public void MissInput()
    {
        currentInputs.Dequeue().FailInput();
        FailInput();
        
        guitarManager.RemoveInput();
        guitarManager.CheckWin();
    }

    private void FailInput()
    {
        //DOTween.Complete(answerScript.frontImage);
        if (!DOTween.IsTweening(answerScript.frontImage))
        {
            GameManager.Instance.FadeImageColorInOut(GameManager.Instance.buttonRed, 0.075f, 0.25f, answerScript.frontImage);
        }


        guitarManager.FailInput();

    }

    private void CorrectInput()
    {
        DOTween.Complete(answerScript.frontImage);
       
       GameManager.Instance.FadeImageColorInOut(GameManager.Instance.buttonGreen, 0.075f, 0.25f, answerScript.frontImage);
       

        currentInputs.Dequeue().CorrectInput();
        
        guitarManager.RemoveInput();
        guitarManager.CheckWin();

    }
    

    public void SetInput(GUITARInput newInput)
    {
        // Sets up the input to this answer
        newInput.Setup(inputSpawner, this);
        
        // Adds this answers input queue
        currentInputs.Enqueue(newInput);
    }

    public void OnClick()
    {
        if (currentInputs.Count > 0 && currentInputs.Peek().IsOnLine())
        {
            CorrectInput();
        }
        else
        {
            FailInput();
        }
    }

    public RectTransform GetInputSpawner()
    {
        return inputSpawner;
    }

    public void ClearInputs()
    {
        foreach (GUITARInput input in currentInputs)
        {
            input.Destroy();
        }
    }
    
}
