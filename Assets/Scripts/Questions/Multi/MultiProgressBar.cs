using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiProgressBar : MonoBehaviour
{
    private List<MultiProgressNotch> progressNotches = new();
    [SerializeField] private GameObject progressNotch;
    [SerializeField] private QuestionMultiGeneric questionMultiScript;

    private void Awake()
    {
        for(int i = 0; i < questionMultiScript.GetPhaseCount(); i++)
            progressNotches.Add(Instantiate(progressNotch, transform).GetComponent<MultiProgressNotch>());
    }

    private void OnEnable()
    {
        questionMultiScript.OnStart += OnStart;
        questionMultiScript.OnFail += OnFail;
        questionMultiScript.OnReset += OnReset;
        questionMultiScript.OnCorrect += OnCorrect;
        questionMultiScript.OnWin += OnWin;
    }

    public void OnCorrect()
    {
        progressNotches[questionMultiScript.CurrentPhase-1].Completed();
        progressNotches[questionMultiScript.CurrentPhase].Selected();
    }

    public void OnFail()
    {
        progressNotches[questionMultiScript.CurrentPhase].Failed();
    }

    public void OnStart()
    {
        for(int i = questionMultiScript.GetPhaseCount(); i < progressNotches.Count; i++)
        {
            progressNotches[i].gameObject.SetActive(false); 
        }

        progressNotches[0].Selected();
    }

    public void OnWin()
    {
        progressNotches[questionMultiScript.CurrentPhase-1].Completed();
    }

    public void OnReset()
    {
        ResetNotches();
    }
    private void ResetNotches()
    {
        foreach(MultiProgressNotch notch in progressNotches)
        {
            notch.Reset();
        }
    }
}