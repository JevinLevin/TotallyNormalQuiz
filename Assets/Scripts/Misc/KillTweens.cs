using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KillTweens : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
