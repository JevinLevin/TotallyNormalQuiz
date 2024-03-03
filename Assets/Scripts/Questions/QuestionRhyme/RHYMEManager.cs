using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RHYMEManager : MonoBehaviour
{
    [SerializeField] private string[] words;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private QuestionTimer timer;
    private QuestionMultiGeneric questionScript;

    private void Awake()
    {
        questionScript = GetComponent<QuestionMultiGeneric>();
    }

    private void OnEnable()
    {
        questionScript.OnSetQuestion += SetWord;
        questionScript.OnCorrect += timer.CountdownCorrect;
    }

    private void OnDisable()
    {
        questionScript.OnSetQuestion -= SetWord;
    }

    private void SetWord()
    {
        text.text = words[questionScript.CurrentPhase];
    }
}
