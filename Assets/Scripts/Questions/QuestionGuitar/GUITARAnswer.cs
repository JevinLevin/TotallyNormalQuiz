using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GUITARAnswer : MonoBehaviour
{
    [SerializeField] private AnswerGeneric answerScript;
    [SerializeField] private GUITARManager guitarManager;
    [SerializeField] private RectTransform inputSpawner;
    [SerializeField] private Sprite inputSprite;
    private Queue<GUITARInput> currentInputs;

    private const float buttonDelay = 0.2f;
    private float buttonTimer;


    private void Awake()
    {
        currentInputs = new Queue<GUITARInput>();
    }

    void Update()
    {
        buttonTimer -= Time.deltaTime;
    }


    public List<GUITARInput> GetInputs()
    {
        return currentInputs.ToList();
    }

    public void ClearInputs()
    {
        currentInputs.Clear();
    }
    

    public void MissInput()
    {
        // Remove missed input from queue
        currentInputs.Dequeue().FailInput();
        
        
        // Generic input fail function
        FailInput();
    }

    // If an input is failed
    private void FailInput()
    {
        // Cancels the current tween if its already flashing red
        DOTween.Complete(answerScript.frontImage);
        GameManager.FadeImageColorInOut(GameManager.ButtonRed, 0.075f, 0.2f, answerScript.frontImage);
        

        // Main fail input function
        guitarManager.FailInput();

    }

    private void CorrectInput()
    {
        // Cancels the current tween if its already flashing red
        DOTween.Complete(answerScript.frontImage);
        GameManager.FadeImageColorInOut(GameManager.ButtonGreen, 0.075f, 0.2f, answerScript.frontImage);
       
        // Remove correct input from queue
        currentInputs.Dequeue().CorrectInput();
        

    }

    public void OnClick()
    {
        // If the button is on cooldown
        if (!(buttonTimer <= 0)) return;
        
        if (currentInputs.Count > 0 && currentInputs.Peek().OnLine)
        {
            CorrectInput();
            buttonTimer = buttonDelay;
        }
        else
        {
            FailInput();
            buttonTimer = 0.1f;
        }
    }
    
    public void SetInput(GUITARInput newInput, GUITARManager manager)
    {
        // Sets up the input to this answer
        newInput.Setup(inputSpawner, this, manager, inputSprite);
        
        // Adds this answers input queue
        currentInputs.Enqueue(newInput);
    }

    public RectTransform GetInputSpawner()
    {
        return inputSpawner;
    }
    
    
}
