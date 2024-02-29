using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class COUNTDOWNManager : MonoBehaviour
{
    


    [Title("Components")]
    private QuestionMultiGeneric questionMultiScript;
    [SerializeField] private TextMeshProUGUI phaseNumberDisplay;
    [SerializeField] private QuestionTimer timer;

    void Awake()
    {
        questionMultiScript = GetComponent<QuestionMultiGeneric>();
    }
    
    private void OnEnable()
    {
        questionMultiScript.OnSetQuestion += SetPhaseNumber;
        questionMultiScript.OnCorrect += QuestionCorrect;
        questionMultiScript.OnWin += QuestionWin;
    }
    
    private void OnDisable()
    {
        questionMultiScript.OnSetQuestion -= SetPhaseNumber;
        questionMultiScript.OnCorrect -= QuestionCorrect;
        questionMultiScript.OnWin -= QuestionWin;
    }

    public void SetPhaseNumber()
    {
        phaseNumberDisplay.text = (questionMultiScript.GetPhaseCount() -1- questionMultiScript.CurrentPhase).ToString();
    }

    private void QuestionWin()
    {
        phaseNumberDisplay.text = "-";
    }

    private void QuestionCorrect()
    {
        // Set question title to current number
        SetPhaseNumber();
        
        timer.CountdownCorrect();
    }

}
