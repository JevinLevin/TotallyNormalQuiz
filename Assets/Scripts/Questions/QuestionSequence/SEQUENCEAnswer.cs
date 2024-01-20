using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SEQUENCEAnswer : MonoBehaviour
{

    public enum AnswerColors
    {
        Red,
        Blue,
        Green,
        Yellow,
        DarkBlue,
        Pink,
        Orange,
        Aqua,
        Purple
    }

    public AnswerColors answerColor;

    [SerializeField] public Image frontImage;
    [SerializeField] public Image backImage;
    [SerializeField] private int answerNumber;
    private Color defaultFrontColor;
    private Color defaultBackColor;
    private Sequence winSequence;

    void Awake()
    {
        defaultFrontColor = frontImage.color;
        defaultBackColor = backImage.color;
    }

    public void Reset()
    {
        backImage.color = defaultBackColor;
    }

    public Color GetColor()
    {
        if (answerColor == AnswerColors.Red) return GameManager.ButtonRed;
        if (answerColor == AnswerColors.Blue) return GameManager.ButtonBlue;
        if (answerColor == AnswerColors.Green) return GameManager.ButtonGreen;
        if (answerColor == AnswerColors.Yellow) return GameManager.ButtonYellow;
        if (answerColor == AnswerColors.DarkBlue) return GameManager.ButtonDarkBlue;
        if (answerColor == AnswerColors.Pink) return GameManager.ButtonPink;
        if (answerColor == AnswerColors.Orange) return GameManager.ButtonOrange;
        if (answerColor == AnswerColors.Aqua) return GameManager.ButtonAqua;
        if (answerColor == AnswerColors.Purple) return GameManager.ButtonPurple;

        // If it somehow fails
        return Color.black;
    }

    public int GetNumber()
    {
        return answerNumber;
    }

    public void ResetFrontImage()
    {
        frontImage.DOColor(defaultFrontColor, 0.15f);
    }
}
