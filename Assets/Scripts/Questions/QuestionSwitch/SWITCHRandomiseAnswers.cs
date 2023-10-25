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
    
    public Vector3 mouseDelta = Vector3.zero;
    private Vector3 lastMousePosition = Vector3.zero;
    public bool mouseSpeed;

    void Update()
    {
        
        mouseDelta = Input.mousePosition - lastMousePosition;

        lastMousePosition = Input.mousePosition;

        if (mouseDelta.x > 1 || mouseDelta.y > 1)
        {
            mouseSpeed = true;
        }
        else
        {
            mouseSpeed = false;
        }
        
        // If the question is active
        if(active)
        {
            // If the player clicks anywhere in the scene
            if(Input.GetMouseButtonDown(0))
            {
                // This variable ensures the player cant spam click the right answer as the level loads
                clicked = true;
            }
        }
    }

    // Rearranges the order of answers
    public void RandomiseAnswers()
    {
        // Converts the answers into a list and shuffles it
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

        // Sets the answers according to the shuffled list
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

