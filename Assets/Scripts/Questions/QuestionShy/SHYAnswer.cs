using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SHYAnswer : MonoBehaviour
{
    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private AnswerGeneric answerScript;
    [SerializeField] private RectTransform answerRect;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform[] rectTransforms;

    private bool focus;
    private bool hover;
    private bool started = false;
    private bool ended;

    void Start()
    {
        Hide();
        started = true;
    }

    void Update()
    {
        if(hover && Input.GetMouseButtonDown(0))
        {
            OnAnswerClicked();
        }


        if(started)
        {
            started = false;
        }
        started = true;
    }

    public void OnPointedEnter()
    {
        hover = true;
    }

    public void OnPointedExit()
    {
        hover = false;
    }



    void OnApplicationFocus(bool focus)
    {
        if(focus && started && !ended)
        {
            Unhide();
        }
    }   

    private void OnAnswerClicked()
    {
        ended = true;

        DOTween.Kill("shyTween");
        canvasGroup.DOFade(1.0f,0.25f).SetId("answerTween");
        questionScript.ClickAnswerGeneric(answerScript, true);
    }

    private void Unhide()
    {
        Reposition();

        canvasGroup.interactable = true;

        DOTween.Complete("shyTween");
        canvasGroup.DOFade(1.0f,0.1f).SetId("answerTween").SetId("shyTween").OnComplete(() =>
        canvasGroup.DOFade(0.0f,1f).SetId("answerTween").SetId("shyTween").SetEase(Ease.InQuad).OnComplete(() =>
        Hide()));

    }

    private void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }

    public void Reposition()
    {
        float xWidth=  canvasScaler.referenceResolution.x * canvas.scaleFactor;
        float yWidth = canvasScaler.referenceResolution.y * canvas.scaleFactor;
        do
        {
            answerRect.anchoredPosition = new Vector2(Random.Range(-xWidth/2,xWidth/2),Random.Range(-yWidth/2,yWidth/2)); 
        } while(!answerRect.IsFullyVisibleFrom() || CheckOverlap());
    }

    private bool CheckOverlap()
    {
        foreach(RectTransform rect in rectTransforms)
        {
            if (answerRect.Overlaps(rect))
            {
                return true;
            }
        }
        return false;
    }

}
