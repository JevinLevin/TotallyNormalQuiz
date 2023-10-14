using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuitAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform maskRect;
    [SerializeField] private float quitLength;
    public IEnumerator PlayAnimation()
    {
        maskRect.DOScale(0.0f, quitLength).SetId("QuitAnimation").SetEase(Ease.InOutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(quitLength + 0.25f);
        
        Application.Quit();
        #if UNITY_EDITOR
            print("quit");
            maskRect.localScale = Vector3.one;
            gameObject.SetActive(false);
        #endif

    }
}
