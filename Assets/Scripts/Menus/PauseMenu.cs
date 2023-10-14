using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnPause(bool pause)
    {
        gameObject.SetActive(pause);
        Time.timeScale = System.Convert.ToSingle(!pause);
    }
}
