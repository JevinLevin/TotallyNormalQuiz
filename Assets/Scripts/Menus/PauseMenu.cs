using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInLength;
    void Awake()
    {
        slider.value = GameManager.volume;
    }
    
    public void OnPause(bool pause)
    {
        DOTween.Complete("PauseTween");
        
        gameObject.SetActive(pause);
        if(pause) FadeMenu();
        Time.timeScale = System.Convert.ToSingle(!pause);
    }

    private void FadeMenu()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f, fadeInLength).SetId("PauseTween").SetUpdate(true);
    }

    public void OnResume()
    {
        GameManager.Instance.OnPause();
    }

    public void OnQuit()
    {
        GameManager.Instance.QuitGame();
    }
}
