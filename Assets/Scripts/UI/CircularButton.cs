using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircularButton : MonoBehaviour
{

    [SerializeField] private Image button;
    [SerializeField] private Image border;

    void Awake()
    {
        button.alphaHitTestMinimumThreshold = 0.5f;
        border.alphaHitTestMinimumThreshold = 0.5f;
    }



}
