using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterNumber : MonoBehaviour
{
    [SerializeField]
    private InputField x;
    [SerializeField]
    private InputField y;
    [SerializeField]
    private InputField number;

    [SerializeField]
    private SudokuManager sudokuManager;

    public void SetNumber()
    {
        if (x.text != "" && y.text != "" && number.text != "")
        {
            sudokuManager.SetNumber(Int32.Parse(x.text), Int32.Parse(y.text), number.text);
        }
    }
}
