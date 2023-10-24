using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class COLORDRAGDragAnswer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{

    [SerializeField] private COLORDRAGAnswer answerScript;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;
    public UnityEvent HoverEnd;
    public UnityEvent HoverStart;

    public void OnBeginDrag(PointerEventData data)
    {
        if(!GameManager.Instance.restarting)
        {
        answerScript.canClick = false;
        HoverEnd?.Invoke();
        }
    }

    public void OnDrop(PointerEventData data)
    {
        if(!GameManager.Instance.restarting)
        {
        answerScript.canClick = true;
        HoverStart?.Invoke();
        }
        
    }

    public void OnEndDrag(PointerEventData data)
    {
        if(!GameManager.Instance.restarting)
        {
        answerScript.canClick = true;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        rectTransform.anchoredPosition += data.delta / canvas.scaleFactor;
        UIMustInScreen(rectTransform);
    }

    private void UIMustInScreen(RectTransform target) {
       
        RectTransform canvasRect = canvas.transform as RectTransform;
        RectTransform parentRect = target.parent.GetComponent<RectTransform>();
        Camera        cam        = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        Vector2       screenPos  = RectTransformUtility.WorldToScreenPoint(cam, target.position);
 
        float minX = target.pivot.x * target.rect.size.x;
        float maxX = (canvasRect.rect.size.x*canvas.scaleFactor) - (1 - target.pivot.x) * target.rect.size.x;
        float minY = target.pivot.y * target.rect.size.y;
        float maxY = (canvasRect.rect.size.y*canvas.scaleFactor) - (1 - target.pivot.y) * target.rect.size.y;
       
        screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
        screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);
 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPos, cam, out Vector2 anchoredPos);
        target.localPosition = anchoredPos;
    }


}
