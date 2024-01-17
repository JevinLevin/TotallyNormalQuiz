using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MULTIPLEAnswerHandler : MonoBehaviour
{

    [SerializeField] private QuestionGeneric questionScript;

    private AnswerGeneric[] answerList;
    private string[] capitals;

    private string filePath;

    void Awake()
    {
        // Grab all answer objects
        answerList = GameObject.FindObjectsOfType<AnswerGeneric>();
        questionScript.Answers.AddRange(answerList);
        SetAnswers();
    }

    private void SetAnswers()
    {
        // Sets filepath to text file
        filePath =  Application.streamingAssetsPath + "/capitals.txt";

        // Get list of capital cities and shuffle them
        capitals = File.ReadAllLines(filePath);
        capitals.Shuffle();

        // Set every answer to match the list
        for(int i = 0; i < 196; i++)
        {
            answerList[i].SetText(capitals[i]);
            answerList[i].SetCorrect(false);
        }

        // Replace a random answer with Brussels
        int index = Random.Range(0, answerList.Length);
        // Makes sure the answer isnt on the outside border
        while(!ValidIndex(index))
        {
            index = Random.Range(0, answerList.Length);
        }
        answerList[index].SetText("Brussels");
        answerList[index].SetCorrect(true);
    }

    private bool ValidIndex(int index)
    {  
        if(
            index < 14 ||
            index > 182 ||
            index % 14 == 0 ||
            index % 14 == 13
        )
        {
            return false;
        }
        return true;

    }
}
