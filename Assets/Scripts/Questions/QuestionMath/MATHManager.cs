using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MATHManager : MonoBehaviour
{
    [SerializeField] private AnswerGeneric answerScript;
    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private TMP_InputField inputField;

    void Start()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void OnReset()
    {
        inputField.text = "";
        inputField.interactable = true;
    }

    public void Update()
    {
        if (!inputField.isFocused)
        {
            inputField.Select();
            inputField.ActivateInputField();
        }

        //if (inputField.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        //{
        //    OnEnter();
        //}
    }

    public void OnClickAnswer()
    {
        string input = inputField.text;
        input = input.Replace(" ", "");

        if (input == "56")
        {
            Win();
        }
        else
        {
            inputField.interactable = false;
            questionScript.ClickAnswerGeneric(answerScript, false);
        }
    }

    private void Win()
    {
        inputField.gameObject.SetActive(false);
        
        questionScript.ClickAnswerGeneric(answerScript,true);
    }

}
