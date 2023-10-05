using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SWITCHRandomiseAnswers : MonoBehaviour
{

    [SerializeField] private List<SWITCHClickCheck> answerList;
    private List<SWITCHAnswerClass> newAnswerList;

    public bool clicked;
    private bool active = true;

    void Update()
    {
        if(active)
        {
            if(Input.GetMouseButtonDown(0))
            {
                clicked = true;
            }
        }
    }

    public void RandomiseAnswers()
    {
        newAnswerList = new List<SWITCHAnswerClass>();
        foreach(SWITCHClickCheck answer in answerList)
        {
            newAnswerList.Add(new SWITCHAnswerClass(answer.correctAnswer,answer.answerText.text));
        }

        newAnswerList.Shuffle();

        // This loop ensures no answers shuffle back to the same location
        while(!CheckRepeat())
        {
            newAnswerList.Shuffle();
        }

        for(int i = 0; i < answerList.Count; i++)
        {
            answerList[i].correctAnswer = newAnswerList[i].correct;
            answerList[i].SetCorrect(newAnswerList[i].correct);
            answerList[i].answerText.text = newAnswerList[i].text;
        }



    }

    // Loops through all answers, and if any answers are in the same position return false
    private bool CheckRepeat()
    {

        for(int i = 0; i < answerList.Count; i++)
        {
            if(answerList[i].answerText.text == newAnswerList[i].text)
            {
                return false;
            }
        }
        return true;
        

    }

    public void OnFail()
    {
        active = false;
        clicked = false;
    }

    public void OnReset()
    {
        active = true;
    }


}

