using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MongoDB.Bson.Serialization.Attributes;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        var client = new MongoClient("mongodb+srv://test:tfg@cluster0-ayt30.mongodb.net/test?retryWrites=true&w=majority");
        var database = client.GetDatabase("test");
        var collection = database.GetCollection<BsonDocument>("test2");

        //var document = new BsonDocument { { "userId", 1234},
        //    { "number", 1}
        //};

        var testClass = new TestClass(1234, 1);

        var document = testClass.ToBsonDocument();

        var filter = Builders<BsonDocument>.Filter.Eq("userId", 1234);

        //var studentDocument = collection.Find(document).FirstOrDefault();
        Debug.Log(document.ToJson().ToString());

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseGame()
    {
        Application.Quit();
    }


    [BsonDiscriminator("test")]
    public class TestClass
    {
        [BsonElement("userId")]
        int userId;
        [BsonElement("number")]
        int number;

        public TestClass(int userId, int number)
        {
            this.userId = userId;
            this.number = number;
        }
    }
}
