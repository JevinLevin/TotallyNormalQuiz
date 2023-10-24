using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GUITARLives : MonoBehaviour
{
    [SerializeField] private GUITARManager guitarManager;
    [SerializeField] private GameObject livesObject;
    private List<Image> lives = new();
    private Color defaultLifeColor;

    void Start()
    {
        for (int i = 0; i < guitarManager.GetLives(); i++)
        {
            lives.Add(Instantiate(livesObject, transform.position, Quaternion.identity, transform).GetComponent<Image>());
        }

        if (lives.Count > 0) defaultLifeColor = lives[0].color;
    }

    public void LoseLife(int currentLife)
    {
        RemoveLife(lives[currentLife - 1]);
    }

    private void RemoveLife(Image life)
    {
        life.DOColor(GameManager.Instance.buttonRed, 0.5f);
        life.DOFade(0.5f, 0.5f);
    }

    public void ResetLives()
    {
        foreach (Image life in lives)
        {
            life.color = defaultLifeColor;
        }
    }

    public void Win()
    {
        foreach (Image life in lives)
        {
            life.DOColor(GameManager.Instance.buttonGreen, 0.5f);
        }
    }
}
