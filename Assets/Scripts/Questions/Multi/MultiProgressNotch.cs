using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiProgressNotch : MonoBehaviour
{
    private Image image;
    private Color defaultButtonColor;

    void Awake()
    {
        image = GetComponent<Image>();
        defaultButtonColor = image.color;
    }

    public void Selected()
    {
        GameManager.FadeImageColor(GameManager.ButtonHoverColor, 0.15f, image);
    }

    public void Completed()
    {
        GameManager.FadeImageColor(GameManager.ButtonGreen, 0.15f, image);
    }

    public void Failed()
    {
        GameManager.FadeImageColor(GameManager.ButtonRed, 0.15f, image);
    }

    public void Reset()
    {
        image.color = defaultButtonColor;
    }
}