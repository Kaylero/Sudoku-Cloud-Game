using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserMessage : MonoBehaviour
{
    private static Text messageText;

    public void Start()
    {
        messageText = GetComponent<Text>();
    }

    public static void ShowMessage(string message)
    {
        messageText.text = message;
    }
}
