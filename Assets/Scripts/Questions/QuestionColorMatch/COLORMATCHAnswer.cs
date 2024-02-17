using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class COLORMATCHAnswer : MonoBehaviour
{
    private AnswerGeneric answerScript;
    
    
    public float hueSpeed;
    public bool isShifting;

    private Color defaultText;
    private Color defaultColor;

    private void Awake()
    {
        answerScript = GetComponent<AnswerGeneric>();

        defaultText = answerScript.GetTextColor();
        defaultColor = answerScript.frontImage.color;
    }

    private void Update()
    {
        if (!isShifting)
            return;

        Color currentTextColor = answerScript.GetTextColor();
        Color.RGBToHSV(currentTextColor, out float textH, out float textS, out float textV);
        textH += Time.deltaTime / hueSpeed;
        answerScript.SetTextColor(Color.HSVToRGB(textH, textS, textV));
        
        Color currentFrontColor = answerScript.frontImage.color;
        Color.RGBToHSV(currentFrontColor, out float frontH, out float frontS, out float frontV);
        frontH += Time.deltaTime / hueSpeed;
        answerScript.frontImage.color = Color.HSVToRGB(frontH, frontS, frontV);

    }

    public void Reset()
    {
        answerScript.SetTextColor(defaultText);
        answerScript.frontImage.DOColor(defaultColor, 0.2f);
    }
}
