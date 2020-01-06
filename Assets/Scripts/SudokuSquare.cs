using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuSquare : MonoBehaviour
{
    private Text number;
    private Image background;

    private void Start()
    {
        number = GetComponentInChildren<Text>();
        background = GetComponent<Image>();
    }

    public void SetNumber(string number)
    {
        this.number.text = number;
    }

    public string GetNumber()
    {
        return number.text;
    }

    public void ChangeColor(Color color)
    {
        background.color = color;
    }
    
}
