using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class LoadingRectangle : MonoBehaviour
{
    
    // THIS CODE FUCKING SUCKS AND IS PROBABLY THE WORST IVE EVER WRITTEN BUT WHO CARES
    
    [SerializeField] private LineRenderer lineRenderer1;
    [SerializeField] private LineRenderer lineRenderer2;
    [SerializeField] private LineRenderer lineRenderer3;
    [SerializeField] private LineRenderer lineRenderer4;

    private float maxLengthX;
    private float maxLengthY;
    private float totalLength;
    private Vector3[] oldPositions1;
    private Vector3[] oldPositions2;
    private Vector3[] oldPositions3;
    private Vector3[] oldPositions4;
    private int currentIndex;

    void Start()
    {
        oldPositions1 = new Vector3[lineRenderer1.positionCount];
        oldPositions2 = new Vector3[lineRenderer2.positionCount];
        oldPositions3 = new Vector3[lineRenderer3.positionCount];
        oldPositions4 = new Vector3[lineRenderer4.positionCount];

        lineRenderer1.GetPositions(oldPositions1);
        lineRenderer2.GetPositions(oldPositions2);
        lineRenderer3.GetPositions(oldPositions3);
        lineRenderer4.GetPositions(oldPositions4);

        maxLengthX = Mathf.Abs(lineRenderer1.GetPosition(0).x)*2;
        maxLengthY = Mathf.Abs(lineRenderer1.GetPosition(0).y)*2;
        totalLength = GetTotalLength();
        
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }

    [Button]
    private void Reset()
    {
        lineRenderer1.positionCount = oldPositions1.Length;
        lineRenderer1.SetPositions(oldPositions1);
        lineRenderer2.positionCount = oldPositions2.Length;
        lineRenderer2.SetPositions(oldPositions2);
        lineRenderer3.positionCount = oldPositions3.Length;
        lineRenderer3.SetPositions(oldPositions3);
        lineRenderer4.positionCount = oldPositions4.Length;
        lineRenderer4.SetPositions(oldPositions4);
    }

    public void UpdateLine(float progress)
    {
        if (!gameObject.activeSelf)
        {
            Reset();
           gameObject.SetActive(true);
        }
        
        float currentLength = Mathf.Lerp(0, totalLength, progress);
        if (currentLength > maxLengthX + (maxLengthY * 2))
        {
            currentIndex = 4;
            MovePoint(currentIndex, -(currentLength - (maxLengthX + maxLengthY * 2))+maxLengthX/2, "x");
        }
        else if (currentLength > maxLengthX + maxLengthY)
        {
            if(currentIndex == 4) RemoveLastIndex(currentIndex); 
            currentIndex = 3;
            MovePoint(currentIndex, (currentLength - (maxLengthX + maxLengthY))-maxLengthY/2, "y");
        }
        else if (currentLength > maxLengthY)
        {
            if(currentIndex == 3) RemoveLastIndex(currentIndex); 
            currentIndex = 2;
            MovePoint(currentIndex, (currentLength - (maxLengthY))-maxLengthX/2, "x");
        }
        else
        {
            if(currentIndex == 2) RemoveLastIndex(currentIndex); 
            currentIndex = 1;
            MovePoint(currentIndex, -(currentLength)+maxLengthY/2 , "y");
        }
    }

    private float GetTotalLength()
    {
         return Mathf.Abs(maxLengthX)*2 + Mathf.Abs(maxLengthY)*2;
    }

    private void MovePoint(int pointIndex, float pointPosition, string axis)
    {
        if (pointIndex == 4)
        {
            lineRenderer4.SetPosition(1,new Vector2(pointPosition, lineRenderer4.GetPosition(1).y));
        }
        if (pointIndex == 3)
        {
            lineRenderer3.SetPosition(1,new Vector2(lineRenderer3.GetPosition(1).x, pointPosition));
        }
        if (pointIndex == 2)
        {
            lineRenderer2.SetPosition(1,new Vector2(pointPosition, lineRenderer2.GetPosition(1).y));

        }
        if (pointIndex == 1)
        {
            lineRenderer1.SetPosition(1,new Vector2(lineRenderer1.GetPosition(1).x, pointPosition));
        }
    }

    private void RemoveLastIndex(int index)
    {
        if (index == 4)
        {
            lineRenderer4.positionCount = 0;
        }
        if (index == 3)
        {
            lineRenderer3.positionCount = 0;
        }
        if (index == 2)
        {
            lineRenderer2.positionCount = 0;
        }
    }

    public void End()
    {
        gameObject.SetActive(false);
    }

    public void FadeOut(float duration)
    {
        lineRenderer1.DOColor(new Color2(Color.white, Color.white), new Color2(new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f)), duration);
        lineRenderer2.DOColor(new Color2(Color.white, Color.white), new Color2(new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f)), duration);
        lineRenderer3.DOColor(new Color2(Color.white, Color.white), new Color2(new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f)), duration);
        lineRenderer4.DOColor(new Color2(Color.white, Color.white), new Color2(new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f)), duration);
    }
}
