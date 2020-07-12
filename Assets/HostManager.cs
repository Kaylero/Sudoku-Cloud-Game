using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    MongoClient client;
    IMongoDatabase database;

    IMongoCollection<BsonDocument> sudokuCollection;
    IMongoCollection<BsonDocument> squareCollection;
    IMongoCollection<BsonDocument> timerCollection;
    IMongoCollection<BsonDocument> responsesCollection;

    private float timer = 40;

    public void Start()
    {
        client = new MongoClient("mongodb+srv://test:tfg@cluster0-ayt30.mongodb.net/test?retryWrites=true&w=majority");
        database = client.GetDatabase("test");
        sudokuCollection = database.GetCollection<BsonDocument>("testHost");

        //Create new sudoku
        CreateNewSudoku(sudokuCollection);

        //Clean response collection
        responsesCollection = database.GetCollection<BsonDocument>("responses");
        ClearCollection(responsesCollection);

        //CreateNewSquare
        squareCollection = database.GetCollection<BsonDocument>("square");
        UploadNewSquare(squareCollection);

        //Start timer
        timerCollection = database.GetCollection<BsonDocument>("testHostTimer");
        ClearCollection(timerCollection);

        var timerDocument = new TimerDocument(timer);
        UploadTimer(timerCollection, timerDocument);
    }

    private void CreateNewSudoku(IMongoCollection<BsonDocument> sudokuCollection)
    {
        var sudokuGenerator = new SudokuGenerator();
        var newSudoku = sudokuGenerator.GenerateSudoku(5, 9);
        var sudokuDocument = new SudokuDocument(newSudoku);

        UploadSudoku(sudokuCollection, sudokuDocument);

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

            UploadTimer(timerCollection, timerDocument);
        }

        serverTimer.text = timerCollection.Find(new BsonDocument()).ToList()[0][1].ToString();
    }

    public void OnTurnEnded()
    {
        var responses = responsesCollection.Find(new BsonDocument()).ToList();

        ProcessResponses(responsesCollection, responses, currentSquare);
        ClearCollection(responsesCollection);

        var sudokuDocument = new SudokuDocument(SudokuParser.SudokuSquareListToStringArray(sudokuManager.GetSudoku()));

        UploadSudoku(sudokuCollection, sudokuDocument);
        UploadNewSquare(squareCollection);
    }

    public void ProcessResponses(IMongoCollection<BsonDocument> responsesCollection, List<BsonDocument> responses, Vector2 square)
    {
        var array = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        foreach (var response in responses)
        {
            array[Int32.Parse(response[1].ToString())]++;
        }

        sudokuManager.SetNumber((int)square.x, (int)square.y, GetMostVotedResponse(array).ToString());
    }

    public void UploadTimer(IMongoCollection<BsonDocument> timerCollection, TimerDocument timerDocument)
    {
        ClearCollection(timerCollection);

        timerCollection.InsertOne(timerDocument.ToBsonDocument());
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

    //TBD: How to parse a sudoku from server: sudokuManager.SetNewSudoku(ParseSudokuString(bsonList[0][1].ToString()));

    public void UploadSudoku(IMongoCollection<BsonDocument> sudokuCollection, SudokuDocument sudokuDocument)
    {
        ClearCollection(sudokuCollection);
        sudokuCollection.InsertOne(sudokuDocument.ToBsonDocument());
    }

    public void UploadNewSquare(IMongoCollection<BsonDocument> squareCollection)
    {
        ClearCollection(squareCollection);

        var square = sudokuManager.GetNextIncompleteSquare();
        var squareDocument = new SquareDocument((int)square.x, (int)square.y);
        squareCollection.InsertOne(squareDocument.ToBsonDocument());

        currentSquare = new Vector2(square.x, square.y);
    }

    public void ClearCollection(IMongoCollection<BsonDocument> collection)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;

        collection.DeleteMany(filter);
    }

    //TODO: Move to Util class
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

    //TODO: Move to Util class
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

    //TODO: Move to Util class
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

    //TODO: Move to Util class
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

