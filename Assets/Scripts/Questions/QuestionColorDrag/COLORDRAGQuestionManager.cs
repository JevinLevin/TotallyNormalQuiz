using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class COLORDRAGQuestionManager : MonoBehaviour
{

    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;
    private bool correct;

    [SerializeField] private List<COLORDRAGAnswer> answers;
    [SerializeField] private RectTransform rectRed;
    [SerializeField] private RectTransform rectOrange;
    [SerializeField] private RectTransform rectBlue;
    [SerializeField] private RectTransform rectGreen;
    [SerializeField] private RectTransform overlapRectTransform;
    [SerializeField] private GameObject overlapRectMask;
    [SerializeField] private Image overlapRectImage;

    private float scaleX;
    private float scaleY;
    private float aspectX;
    private float aspectY;

    void Start()
    {
        CalculateScale();
    }

    private void CalculateScale()
    {
        scaleX = canvasScaler.referenceResolution.x / Screen.width;
        scaleY = canvasScaler.referenceResolution.y / Screen.height;   
    }


    public void OnReset()
    {
        foreach(COLORDRAGAnswer answer in answers)
        {
            answer.Reset();
        }
        //overlapRectTransform.gameObject.SetActive(false);
    }

    public void ClickAnswer(AnswerGeneric answer)
    {

        if(CheckCorrect())
        {
            ClickWin();
        }
        else
        {
            ClickFail();
        }

    }

    private void ClickWin()
    {
        //FadeAnswers(GameManager.Instance.buttonGreen);

        Rect overlapRect = CheckOverlap();

        overlapRectTransform.position = overlapRect.position;  

        overlapRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, overlapRect.width);
        overlapRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, overlapRect.height); 

        RectTransform overlapObj = Instantiate(overlapRectMask, rectGreen.transform.position, Quaternion.identity, rectGreen).GetComponent<RectTransform>();
        overlapObj.transform.SetParent(overlapRectTransform.transform, true);
        overlapObj.SetSiblingIndex(0);

        overlapRectImage.DOFade(0.0f,0.0f).OnComplete(() =>
        overlapRectImage.DOFade(1f,0.5f));
        overlapRectImage.color = GameManager.Instance.buttonGreen;
        overlapRectTransform.gameObject.SetActive(true);

        questionScript.GenericAnswerCorrect();
    }

    private void ClickFail()
    {
        FadeAnswers(GameManager.Instance.buttonRed);

        questionScript.GenericAnswerWrong();
    }

    private void FadeAnswers(Color fadeColor)
    {
        foreach(AnswerGeneric answer in questionScript.answers)
        {
            GameManager.Instance.FadeImageColor(fadeColor, 0.25f, answer.frontImage);
        }
    }

    private bool CheckCorrect()
    {
        int correctCount = 0;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach(RaycastResult result in results)
        {
            if(result.gameObject.name == "Button Red") correctCount++;
            if(result.gameObject.name == "Button Blue") correctCount++;
            if(result.gameObject.name == "Button Orange") correctCount++;
            if(result.gameObject.name == "Button Green") correctCount = 0;
        }
        if(correctCount==3) return true;
        else return false;
    }

    private Rect CheckOverlap()
    {
        CalculateScale();

        Rect rect1 = new Rect(rectRed.position.x * scaleX, rectRed.position.y * scaleY, rectRed.rect.width, rectRed.rect.height);
        Rect rect2 = new Rect(rectBlue.position.x * scaleX, rectBlue.position.y * scaleY, rectBlue.rect.width, rectBlue.rect.height);
        Rect rect3 = new Rect(rectOrange.position.x * scaleX, rectOrange.position.y * scaleY, rectOrange.rect.width, rectOrange.rect.height);

        // Initialize an empty rectangle for the overlapping area
        Rect overlappingRect = Rect.zero;

        overlappingRect = Rect.MinMaxRect(
            Mathf.Max(rect1.xMin, rect2.xMin),
            Mathf.Max(rect1.yMin, rect2.yMin),
            Mathf.Min(rect1.xMax, rect2.xMax),
            Mathf.Min(rect1.yMax, rect2.yMax)
        );

        overlappingRect = Rect.MinMaxRect(
            Mathf.Max(overlappingRect.xMin, rect3.xMin),
            Mathf.Max(overlappingRect.yMin, rect3.yMin),
            Mathf.Min(overlappingRect.xMax, rect3.xMax),
            Mathf.Min(overlappingRect.yMax, rect3.yMax)
        );

        overlappingRect.position = new Vector2((overlappingRect.position.x - rect1.size.x/2 + overlappingRect.size.x/2) / scaleX, (overlappingRect.position.y - rect1.size.y/2 + overlappingRect.size.y/2) / scaleY);
        return overlappingRect;

    }
}
