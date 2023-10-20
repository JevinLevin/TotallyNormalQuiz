using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VOWELManager : MonoBehaviour
{

    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private GameObject answersParent;
    [SerializeField] private AnswerGeneric correctParent;
    
    private bool solving;

    void Start()
    {
        
    }

    public void OnReset()
    {
        answersParent.SetActive(true);
        correctParent.gameObject.SetActive(false);
    }

    public void OnStart()
    {
        solving = true;
    }

    void Update()
    {
        if (solving && Input.GetKeyDown(KeyCode.A))
        {
            Win();
        }
    }

    private void Win()
    {

        correctParent.gameObject.SetActive(true);
        correctParent.canvasGroup.alpha = 0;
        correctParent.canvasGroup.DOFade(1.0f, 0.25f).OnComplete(() =>
            answersParent.SetActive(false));
        
        solving = false;
        
        questionScript.ClickAnswerGeneric(correctParent, true);
    }
}
