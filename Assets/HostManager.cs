using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HostManager : MonoBehaviour
{
    [SerializeField]
    SudokuManager sudokuManager;

    [SerializeField]
    Text localTimer;
    [SerializeField]
    Text serverTimer;

    MongoClient client;
    IMongoDatabase database;

    public float timer = -5f;

    public void Start()
    {
        //TODO: Check if there is a host already

        var sudokuGenerator = new SudokuGenerator();
        var newSudoku = sudokuGenerator.GenerateSudoku(5, 9);

        var sudokuDocument = new SudokuDocument(newSudoku);
        var timerDocument = new TimerDocument(30.5f);
        var coordinatesDocument = new CoordinatesDocument(2, 4);

        var filter = Builders<BsonDocument>.Filter.Empty;

        //var sdocument = sudokuDocument.ToBsonDocument().ToJson();
        var cdocument = coordinatesDocument.ToBsonDocument().ToJson();


        client = new MongoClient("mongodb+srv://test:tfg@cluster0-ayt30.mongodb.net/test?retryWrites=true&w=majority");
        database = client.GetDatabase("test");
        var sudokuCollection = database.GetCollection<BsonDocument>("testHost");

        //Create new sudoku
        sudokuCollection.DeleteMany(filter);
        sudokuCollection.InsertOne(sudokuDocument.ToBsonDocument());

        var bsonList = sudokuCollection.Find(new BsonDocument()).ToList();

        sudokuManager.SetNewSudoku(ParseSudokuString(bsonList[0][1].ToString()));

        //Clean response collection
        var responseCollection = database.GetCollection<BsonDocument>("responses");
        responseCollection.DeleteMany(filter);
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        var filter = Builders<BsonDocument>.Filter.Empty;

        var timerCollection = database.GetCollection<BsonDocument>("testHostTimer");

        if (timer <= 0)
        {
            timer = 40f;

            //Process responses 
            var responseCollection = database.GetCollection<BsonDocument>("responses");

            var responses = responseCollection.Find(new BsonDocument()).ToList();

            var array = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var response in responses)
            {
                array[Int32.Parse(response[1].ToString())]++;
            }

            //TODO: make this get a random instead of the biggest
            int max = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] >= max)
                {
                    max = i;
                }
            }

            Vector2 square = sudokuManager.GetNextIncompleteSquare();
            sudokuManager.SetNumber((int)square.x, (int)square.y, max.ToString());

            responseCollection.DeleteMany(filter);

            //Insert Sudoku

            var sudokuDocument = new SudokuDocument(SudokuParser.SudokuSquareListToStringArray(sudokuManager.GetSudoku()));

            var sudokuCollection = database.GetCollection<BsonDocument>("testHost");
            sudokuCollection.DeleteMany(filter);
            sudokuCollection.InsertOne(sudokuDocument.ToBsonDocument());

            //Create new square 
            var squareCollection = database.GetCollection<BsonDocument>("square");
            squareCollection.DeleteMany(filter);
            square = sudokuManager.GetNextIncompleteSquare();

            var squareDocument = new SquareDocument((int)square.x, (int)square.y);

            squareCollection.InsertOne(squareDocument.ToBsonDocument());
        }
        else
        {
            localTimer.text = timer.ToString();

            var timerDocument = new TimerDocument(timer);

            timerCollection.DeleteMany(filter);
            timerCollection.InsertOne(timerDocument.ToBsonDocument());
        }

        serverTimer.text = timerCollection.Find(new BsonDocument()).ToList()[0][1].ToString();
    }

    static string ConvertStringArrayToString(string[] array)
    {
        // Concatenate all the elements into a StringBuilder.
        StringBuilder builder = new StringBuilder();
        foreach (string value in array)
        {
            builder.Append(value);
            builder.Append('.');
        }
        return builder.ToString();
    }

    public BsonArray ToBsonDocumentArray(IEnumerable list)
    {
        var array = new BsonArray();
        foreach (var item in list)
        {
            array.Add(item.ToBson());
        }
        return array;
    }

    public float GetTimer()
    {
        return timer;
    }

    string lastChar = "";

    public string[] ParseSudokuString(string sudoku)
    {
        List<string> result = new List<string>();

        foreach (char c in sudoku)
        {
            string cStr = c.ToString();

            if (isNumeric(c.ToString()))
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

    public static Boolean isNumeric(string strToCheck)
    {
        Regex rg = new Regex(@"^[0-9]*$");
        return rg.IsMatch(strToCheck);
    }
}
[BsonDiscriminator("square")]
public class SquareDocument
{
    [BsonElement]
    public Int32 x;
    [BsonElement]
    public Int32 y;

    public SquareDocument(Int32 x, Int32 y)
    {
        this.x = x;
        this.y = y;
    }
}

[BsonDiscriminator("response")]
public class ResponseDocument
{
    [BsonElement]
    public Int32 number;

    public ResponseDocument(Int32 number)
    {
        this.number = number;
    }
}

[BsonDiscriminator("sudoku")]
public class SudokuDocument
{
    [BsonElement("sudoku")]
    public string[] sudoku;

    public SudokuDocument(string[] sudoku)
    {
        this.sudoku = sudoku;
    }
}

[BsonDiscriminator("timer")]
public class TimerDocument
{
    [BsonElement("timer")]
    public float timer;

    public TimerDocument(float timer)
    { 
        this.timer = timer;
    }
}

[BsonDiscriminator("coordinates")]
public class CoordinatesDocument
{
    [BsonElement("x")]
    public int x;
    [BsonElement("y")]
    public int y;

    public CoordinatesDocument(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

[Serializable]
public class SudokuJson
{
    public float timer;
    public string[] sudoku;

    public SudokuJson(string[] sudoku, float timer)
    {
        this.sudoku = sudoku;
    }
}

[Serializable]
public class TimerJson
{
    public float timer;

    public TimerJson(float timer)
    {
        this.timer = timer;
    }
}

[Serializable]
public class CoordinatesJson
{
    public int x;
    public int y;

    public CoordinatesJson(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

