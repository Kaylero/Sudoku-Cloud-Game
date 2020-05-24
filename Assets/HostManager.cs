using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public float timer = 30f;

    public void Start()
    {
        //TODO: Check if there is a host already

        var sudokuGenerator = new SudokuGenerator();
        var newSudoku = sudokuGenerator.GenerateSudoku(5, 9);

        var sudokuDocument = new SudokuDocument(newSudoku);
        var timerDocument = new TimerDocument(30.5f);
        var coordinatesDocument = new CoordinatesDocument(2, 4);

        var filter = Builders<BsonDocument>.Filter.Empty;

        var sdocument = sudokuDocument.ToBsonDocument().ToJson();
        var tdocument = timerDocument.ToBsonDocument().ToJson();
        var cdocument = coordinatesDocument.ToBsonDocument().ToJson();

        var xd = JsonUtility.FromJson<SudokuJson>(sudokuDocument.ToBsonDocument().ToJson());
        var xd1 = JsonUtility.FromJson<TimerJson>(timerDocument.ToBsonDocument().ToJson());
        var xd2 = JsonUtility.FromJson<CoordinatesJson>(coordinatesDocument.ToBsonDocument().ToJson());

        client = new MongoClient("mongodb+srv://test:tfg@cluster0-ayt30.mongodb.net/test?retryWrites=true&w=majority");
        database = client.GetDatabase("test");
        var sudokuCollection = database.GetCollection<BsonDocument>("testHost");
        var timerCollection = database.GetCollection<BsonDocument>("testHostTimer");

        sudokuCollection.DeleteMany(filter);
        timerCollection.DeleteMany(filter);

        sudokuCollection.InsertOne(sudokuDocument.ToBsonDocument());
        timerCollection.InsertOne(timerDocument.ToBsonDocument());


        var bsonList = sudokuCollection.Find(new BsonDocument()).ToList();
        var bsonList2 = timerCollection.Find(new BsonDocument()).ToList();

        foreach (var bson in bsonList2)
        {
            Debug.Log(bson.ToString());
            Debug.Log(bson[0].ToString());
            Debug.Log(bson[1].ToString());
        }

        sudokuManager.SetNewSudoku(ParseSudokuString(bsonList[0][1].ToString()));

    }

    public void Update()
    {
        Debug.Log("??");

        timer -= Time.deltaTime;

        var filter = Builders<BsonDocument>.Filter.Empty;
        var timerDocument = new TimerDocument(timer);
        var timerCollection = database.GetCollection<BsonDocument>("testHostTimer");

        if (timer <= 0)
        {
            timer = 30f;
        }
        else
        {
            localTimer.text = timer.ToString();

            timerCollection.DeleteMany(filter);
            timerCollection.InsertOne(timerDocument.ToBsonDocument());
        }

        serverTimer.text = timerCollection.Find(new BsonDocument()).ToList()[0][1].ToString();
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

    bool openedQuote = false;
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

