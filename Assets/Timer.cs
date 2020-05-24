using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    Text text;
    [SerializeField]
    HostManager hostManager;

    void Update()
    {
        text.text = hostManager.GetTimer().ToString();
    }
}
