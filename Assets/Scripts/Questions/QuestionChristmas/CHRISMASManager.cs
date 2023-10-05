using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHRISMASManager : MonoBehaviour
{

    [SerializeField] private QuestionGeneric questionScript;

   public void CheckAnswer(AnswerGeneric answerScript)
   {
    DateTime currentDate = DateTime.Now;
    string month = currentDate.ToString("MM");
    string day = currentDate.ToString("dd");    

    if(month == "12" && day == "25")
    {
        {
            questionScript.ClickAnswerGeneric(answerScript, true);
        }
    }
    else
    {
        {
            questionScript.ClickAnswerGeneric(answerScript, false);
        }
    }
   }

}
