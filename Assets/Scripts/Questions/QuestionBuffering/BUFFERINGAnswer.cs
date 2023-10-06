using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUFFERINGAnswer : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject buffer;

    public void Reset()
    {
        text.SetActive(false);
        buffer.SetActive(true);
    }

    public void NoConnection()
    {
        text.SetActive(true);
        buffer.SetActive(false);
    }
}
