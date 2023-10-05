using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COLORDRAGAnswer : MonoBehaviour
{
    [SerializeField] private COLORDRAGQuestionManager questionScript;
    [SerializeField] private RectTransform rectTransform;

    private Vector3 defaultPosition;

    public bool canClick;

    void Awake()
    {
        canClick = true;

        defaultPosition = rectTransform.anchoredPosition;
    }
    public void OnClickAnswer(AnswerGeneric answer)
    {
       if(canClick) questionScript.ClickAnswer(answer);
    }

    public void Reset()
    {
        rectTransform.anchoredPosition = defaultPosition;
    }
}
