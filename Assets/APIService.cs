using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIService : MonoBehaviour
{
    private static APIService instance;

    MongoClient client;
    IMongoDatabase database;

    IMongoCollection<BsonDocument> sudokuCollection;
    IMongoCollection<BsonDocument> squareCollection;
    IMongoCollection<BsonDocument> timerCollection;
    IMongoCollection<BsonDocument> responsesCollection;

    public static APIService GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        client = new MongoClient("mongodb+srv://test:tfg@cluster0-ayt30.mongodb.net/test?retryWrites=true&w=majority");
        database = client.GetDatabase("test");

        sudokuCollection = database.GetCollection<BsonDocument>("testHost");
        responsesCollection = database.GetCollection<BsonDocument>("responses");
        squareCollection = database.GetCollection<BsonDocument>("square");
        timerCollection = database.GetCollection<BsonDocument>("testHostTimer");
    }

    public List<BsonDocument> FindAllDocuments(Collections collection)
    {
        return FindAllDocuments(GetCollectionFromEnum(collection));
    }

    private List<BsonDocument> FindAllDocuments(IMongoCollection<BsonDocument> collection)
    {
        return collection.Find(new BsonDocument()).ToList();
    }

    public void UploadDocument(Collections collection, BsonDocument document)
    {
        UploadDocument(GetCollectionFromEnum(collection), document);
    }

    private void UploadDocument(IMongoCollection<BsonDocument> collection, BsonDocument document)
    {
        collection.InsertOne(document);
    }

    public void ClearCollection(Collections collection)
    {
        ClearCollection(GetCollectionFromEnum(collection));
    }

    private void ClearCollection(IMongoCollection<BsonDocument> collection)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;

        collection.DeleteMany(filter);
    }

    private IMongoCollection<BsonDocument> GetCollectionFromEnum(Collections collections)
    {
        switch (collections)
        {
            case Collections.Sudoku:
                return sudokuCollection;
            case Collections.Square:
                return squareCollection;
            case Collections.Timer:
                return timerCollection;
            case Collections.Response:
                return responsesCollection;
        }

        return null;
    }
}

public enum Collections
{
    Sudoku,
    Response,
    Square,
    Timer
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
