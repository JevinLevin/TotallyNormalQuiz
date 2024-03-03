using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CATImage : MonoBehaviour, IPointerClickHandler
{
    private bool selected;
    private bool temp;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select(true);
        temp = true;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !temp)
            Select(false);

        temp = false;
        
        if (!selected)
            return;

        if (Input.GetKeyDown(KeyCode.Delete))
            Destroy(gameObject);
    }

    private void Select(bool value)
    {
        selected = value;
        outline.enabled = value;

    }
}
