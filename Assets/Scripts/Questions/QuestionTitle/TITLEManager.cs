using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TITLEManager : MonoBehaviour
{
    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        questionScript.OnStart += ShowTuna;
        questionScript.OnFail += HideTuna;
    }

    public void HideTuna()
    {
        text.gameObject.SetActive(false);
    }

    public void ShowTuna()
    {
        text.gameObject.SetActive(true);
    }

    public void OnAnswerClicked()
    {
        text.gameObject.SetActive(true);
        GameManager.FadeImageColor(GameManager.ButtonGreen, 0.25f, text);

        questionScript.GenericAnswerCorrect();

    }
}
