using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiProgressNotch : MonoBehaviour
{
    [SerializeField] private Image image;
    private Color defaultButtonColor;

    void Awake()
    {
        defaultButtonColor = image.color;
    }

    public void Selected()
    {
        GameManager.Instance.FadeImageColor(GameManager.Instance.buttonHoverColor, 0.15f, image);
    }

    public void Completed()
    {
        GameManager.Instance.FadeImageColor(GameManager.Instance.buttonGreen, 0.15f, image);
    }

    public void Failed()
    {
        GameManager.Instance.FadeImageColor(GameManager.Instance.buttonRed, 0.15f, image);
    }

    public void Reset()
    {
        image.color = defaultButtonColor;
    }
}
