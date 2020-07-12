using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HostManager : MonoBehaviour
{
    [SerializeField]
    SudokuManager sudokuManager;
    Vector2 currentSquare;

    [SerializeField]
    Text localTimer;
    [SerializeField]
    Text serverTimer;

    APIService apiService;

    private float timer = 40;

    public void Start()
    {
        apiService = APIService.GetInstance();

        CreateNewSudoku();

        //Clean response collection
        apiService.ClearCollection(Collections.Response);

        //CreateNewSquare
        UploadNewSquare();

        //Start timer
        apiService.ClearCollection(Collections.Timer);

        var timerDocument = new TimerDocument(timer);
        UploadTimer( timerDocument);
    }

    private void CreateNewSudoku()
    {
        var sudokuGenerator = new SudokuGenerator();
        var newSudoku = sudokuGenerator.GenerateSudoku(5, 9);
        var sudokuDocument = new SudokuDocument(newSudoku);

        ClearAndUploadSudoku(sudokuDocument);

        sudokuManager.SetNewSudoku(newSudoku);
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 40f;

            OnTurnEnded();
            
        }
        else
        {
            localTimer.text = timer.ToString();
            var timerDocument = new TimerDocument(timer);

            UploadTimer(timerDocument);
        }

        serverTimer.text = apiService.FindAllDocuments(Collections.Timer)[0][1].ToString();
    }

    public float GetTimer()
    {
        return timer;
    }

    public void OnTurnEnded()
    {
        var responses = apiService.FindAllDocuments(Collections.Response);

        ProcessResponses(responses, currentSquare);
        apiService.ClearCollection(Collections.Response);

        var sudokuDocument = new SudokuDocument(SudokuParser.SudokuSquareListToStringArray(sudokuManager.GetSudoku()));

        ClearAndUploadSudoku(sudokuDocument);
        UploadNewSquare();
    }

    public void ProcessResponses(List<BsonDocument> responses, Vector2 square)
    {
        var array = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        foreach (var response in responses)
        {
            array[Int32.Parse(response[1].ToString())]++;
        }

        sudokuManager.SetNumber((int)square.x, (int)square.y, GetMostVotedResponse(array).ToString());
    }

    public int GetMostVotedResponse(int[] intArray)
    {
        //TODO: make this get a random instead of the biggest
        int max = 0;
        for (int i = 0; i < intArray.Length; i++)
        {
            if (intArray[i] >= max)
            {
                max = i;
            }
        }

        return max;
    }

    public void UploadTimer(TimerDocument timerDocument)
    {
        apiService.ClearCollection(Collections.Timer);
        apiService.UploadDocument(Collections.Timer, timerDocument.ToBsonDocument());
    }

    public void ClearAndUploadSudoku(SudokuDocument sudokuDocument)
    {
        apiService.ClearCollection(Collections.Sudoku);
        apiService.UploadDocument(Collections.Sudoku, sudokuDocument.ToBsonDocument());
    }

    public void UploadNewSquare()
    {
        apiService.ClearCollection(Collections.Square);

        var square = sudokuManager.GetNextIncompleteSquare();
        var squareDocument = new SquareDocument((int)square.x, (int)square.y);
        apiService.UploadDocument(Collections.Sudoku, squareDocument.ToBsonDocument());

        currentSquare = new Vector2(square.x, square.y);
    }
}