using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerGeneric : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button answerButton;
    [SerializeField] public Image backImage;
    [SerializeField] public Image frontImage;
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] public CanvasGroup canvasGroup;

    [Header("Attributes")]
    [SerializeField] private bool correct;

    private Color defaultTextColor;
    private Color defaultBackColour;
    private Color defaultFrontColour;


    private void Awake()
    {
        defaultTextColor = answerText.color;
        defaultBackColour = backImage.color;
        defaultFrontColour = frontImage.color;
    }

    private void OnDestroy()
    {
        DOTween.Kill("answerTween");
    }

    public void SetDefaultColors()
    {
        answerText.color = defaultTextColor;
        backImage.color = defaultBackColour;
        frontImage.color = defaultFrontColour;
    }


    public void SetCorrect(bool value)
    {
        correct = value;
    }
    public bool GetCorrect()
    {
        return correct;
    }

    public void RandomisePlacement(int length)
    {
        transform.SetSiblingIndex(Random.Range(0,length));
    }

    public void FadeText(float fadeTime)
    {
        answerText.DOFade(0.0f,fadeTime).SetId("answerTween");
    }

    public void SetText(string value)
    {
        answerText.text = value;
    }

    public string GetText()
    {
        return answerText.text;
    }

    public Color GetTextColor()
    {
        return answerText.color;
    }

    public void SetTextColor(Color newColor)
    {
        answerText.color = newColor;
    }
    

}
