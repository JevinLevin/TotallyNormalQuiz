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
    [SerializeField] private Image image;

    public bool OnLine { get; set; }
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
    
    public void Setup(RectTransform inputSpawner, GUITARAnswer guitarAnswer, GUITARManager guitarManager, Sprite sprite)
    {
        // Variables
        OnLine = false;
        correct = false;
        
        // Transform (pos + scale)
        transform.SetParent(inputSpawner);
        transform.localScale = Vector3.one;
        rectTransform.position = inputSpawner.position;
        
        // Sprite
        image.sprite = sprite;
        
        // References
        this.guitarAnswer = guitarAnswer;
        this.guitarManager = guitarManager;
    }

    public void FailInput()
    {
        FadeOut();
    }

    public void CorrectInput()
    {
        // This var stops the exit trigger funtion being called, and the game thinking the input went past the line
        correct = true;

        // Correct animation
        rectTransform.DOScale(new Vector2(1.5f, 1.5f), 0.2f);

        FadeOut();
    }

    private void FadeOut()
    {
        
        canvasGroup.DOFade(0.0f, 0.2f).OnComplete(RemoveInput);
    }

    private void RemoveInput()
    {
        guitarManager.RemoveInput(this);
        guitarManager.CheckWin();
    }

    // Tracks once the object has gone over the line
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnLine = true;
    }

    // Runs once the object has gone past the line
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!correct && guitarManager.IsActive())
        {
            guitarAnswer.MissInput();
        }
    }
}
