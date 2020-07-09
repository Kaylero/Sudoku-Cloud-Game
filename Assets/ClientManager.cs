using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    [SerializeField]
    SudokuManager sudokuManager;

    [SerializeField]
    TextMeshProUGUI instructionsText;
    [SerializeField]
    Text titleTimer;
    [SerializeField]
    Text serverTimer;

    MongoClient client;
    IMongoDatabase database;

    [SerializeField]
    TMP_InputField response;
    [SerializeField]
    GameObject enterNumber;

    void Start()
    {
        client = new MongoClient("mongodb+srv://test:tfg@cluster0-ayt30.mongodb.net/test?retryWrites=true&w=majority");
        database = client.GetDatabase("test");

        var sudokuCollection = database.GetCollection<BsonDocument>("testHost");
        var bsonList = sudokuCollection.Find(new BsonDocument()).ToList();
        sudokuManager.SetNewSudoku(ParseSudokuString(bsonList[0][1].ToString()));

        //Set new square
        var squareCollection = database.GetCollection<BsonDocument>("square");
        var squareBsonList = squareCollection.Find(new BsonDocument()).ToList();
        Debug.Log(squareBsonList[0].ToString());
        instructionsText.text = squareBsonList[0][1].ToString() + ", " + squareBsonList[0][2].ToString();
    }

    public void Update()
    {
        var timerCollection = database.GetCollection<BsonDocument>("testHostTimer");

        try 
        {
            double time = timerCollection.Find(new BsonDocument()).ToList()[0][1].ToDouble();

            if (time > 30)
            {
                UserMessage.ShowMessage("");

                //Set new sudoku
                var sudokuCollection = database.GetCollection<BsonDocument>("testHost");
                var sudokuBsonList = sudokuCollection.Find(new BsonDocument()).ToList();
                sudokuManager.SetNewSudoku(ParseSudokuString(sudokuBsonList[0][1].ToString()));

                //Set new square
                var squareCollection = database.GetCollection<BsonDocument>("square");
                var squareBsonList = sudokuCollection.Find(new BsonDocument()).ToList();
                
                instructionsText.text = squareBsonList[0][1].ToString() + ", " + squareBsonList[0][2].ToString();

                titleTimer.text = "Waiting for new square";
                serverTimer.text = "";
            }
            else
            {
                enterNumber.SetActive(true);
                serverTimer.text = timerCollection.Find(new BsonDocument()).ToList()[0][1].ToString();
            }
        }
        catch 
        {
        }
    }

    string lastChar = "";

    public string[] ParseSudokuString(string sudoku)
    {
        List<string> result = new List<string>();

        foreach (char c in sudoku)
        {
            string cStr = c.ToString();

            if (IsNumeric(c.ToString()))
            {
                lastChar = cStr;
            }
            else if (cStr == ",")
            {
                result.Add(lastChar);
                lastChar = "";
            }
        }

        return result.ToArray();
    }

    public static bool IsNumeric(string strToCheck)
    {
        Regex rg = new Regex(@"^[0-9]*$");
        return rg.IsMatch(strToCheck);
    }

    public void EnterResponse()
    {
        if (response.text == "")
        {
            UserMessage.ShowMessage("Your response must be between 0 and 9.");
            return;
        }

        int number = -99;
        Int32.TryParse(response.text, out number);
        if (!IsNumeric(response.text) || number < 0 || number > 9)
        {
            UserMessage.ShowMessage("Your response must be between 0 and 9.");
            return;
        }

        var responseCollection = database.GetCollection<BsonDocument>("responses");
        var responseDocument = new ResponseDocument(Int32.Parse(response.text));
        responseCollection.InsertOne(responseDocument.ToBsonDocument());

        enterNumber.SetActive(false);

        UserMessage.ShowMessage("You entered " + response.text + ". Waiting for others' respone.");        
    }
}
