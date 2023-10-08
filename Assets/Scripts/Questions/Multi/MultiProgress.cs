using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiProgress : MonoBehaviour
{
    [SerializeField] private List<MultiProgressNotch> progressNotches;
    [SerializeField] private QuestionGenericMulti questionMultiScript;

    public void OnCorrect()
    {
        progressNotches[questionMultiScript.currentPhaseNumber-1].Completed();
        progressNotches[questionMultiScript.currentPhaseNumber].Selected();
    }

    public void OnFail()
    {
        progressNotches[questionMultiScript.currentPhaseNumber].Failed();
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
        progressNotches[questionMultiScript.currentPhaseNumber-1].Completed();
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
