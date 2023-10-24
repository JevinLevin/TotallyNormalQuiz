using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GUITARInput : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool onLine;
    private bool correct;

    private GUITARManager guitarManager;
    private GUITARAnswer guitarAnswer;
        
    void Update()
    {
        if(guitarManager.IsActive()) rectTransform.anchoredPosition -= new Vector2(guitarManager.GetMoveSpeed() * Time.deltaTime, 0.0f);
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 1;
    }
    
    public void Setup(RectTransform inputSpawner, GUITARAnswer guitarAnswer)
    {
        onLine = false;
        correct = false;
        
        transform.SetParent(inputSpawner);
        transform.localScale = Vector3.one;
        SetPosition(inputSpawner.position);
        this.guitarAnswer = guitarAnswer;
    }

    public void FailInput()
    {
        FadeOut();
    }

    public void CorrectInput()
    {
        correct = true;
        FadeOut();
        rectTransform.DOScale(new Vector2(1.5f, 1.5f), 0.2f);
    }

    private void FadeOut()
    {
        
        canvasGroup.DOFade(0.0f, 0.2f).OnComplete(() =>
            Destroy());
    }

    public void SetManager(GUITARManager manager)
    {
        guitarManager = manager;
    }

    public void SetPosition(Vector2 position)
    {
        rectTransform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        onLine = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!correct)
        {
            guitarAnswer.MissInput();
        }
    }

    public bool IsOnLine()
    {
        return onLine;
    }

    public void Destroy()
    {
        guitarManager.inputPool.Release(this);
    }
}
