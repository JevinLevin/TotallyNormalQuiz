using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ALPHABETInput : MonoBehaviour
{
    [SerializeField] private QuestionGeneric questionScript;
    
    private TextMeshProUGUI text;
    private int currentLetter;
    private Color defaultColor;

    private char[] input = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        defaultColor = text.color;
    }

    private void Start()
    {
        ResetProgress();
    }

    private void OnEnable()
    {
        questionScript.OnReset += ResetProgress;
    }

    private void OnDisable()
    {
        questionScript.OnReset -= ResetProgress;
    }

    private void Update()
    {
        if (!questionScript.IsActive)
            return;
        
        if (currentLetter < input.Length)
            CheckInput();
        else
            questionScript.GenericAnswerCorrect();
    }

    private void CheckInput()
    {
        if (Input.inputString.Length == 0)
            return;
        
        if (Input.inputString.Length <= 1 && Input.inputString.Contains(input[currentLetter]))
        {
            NextLetter();
        }   
        else
            Fail();
    }

    private void NextLetter()
    {
        string greenText = "";
        string regularText = "";
        for(int i = 0; i < input.Length; i++)
        {
            if (i <= currentLetter)
                greenText += input[i];
            else
                regularText += input[i];
        }

        greenText = greenText.AddColor(GameManager.ButtonGreen);

        text.text = greenText + regularText;

        currentLetter++;

    }

    private void ResetProgress()
    {
        currentLetter = 0;
        
        string alphabet = "";
        foreach (char letter in input)
            alphabet += letter;

        text.text = alphabet;
        text.color = defaultColor;
    }

    private void Fail()
    {
        text.DOColor(GameManager.ButtonRed, 0.1f);
        questionScript.GenericAnswerWrong();
    }
}
