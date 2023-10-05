using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class ROTATEOrientationChecker : MonoBehaviour
{

    [DllImport("OrientationCheck")]
    private static extern bool OrientationCheck();

    [SerializeField] private QuestionGeneric questionScript;

    public void OnClickAnswer(AnswerGeneric answerScript)
    {
        if(IsFlipped())
        {
            questionScript.ClickAnswerGeneric(answerScript, true);
        }
        else
        {
            questionScript.ClickAnswerGeneric(answerScript, false);
        }
    }


    private bool IsFlipped()
    {
        return OrientationCheck();
    }

}
