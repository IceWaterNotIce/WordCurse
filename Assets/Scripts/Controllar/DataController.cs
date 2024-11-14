using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Data;
using System.Linq;

public class EnglishWord {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Word { get; set; }
    public string Meaning { get; set; }
    // public string Example { get; set; }
    // public string Synonyms { get; set; }
    // public string Antonyms { get; set; }
    // public string Image { get; set; }
    // public string Audio { get; set; }
    // public string Video { get; set; }
    // public string Level { get; set; }
    // public string Category { get; set; }
    // public string SubCategory { get; set; }
    // public string IsFavourite { get; set; }
    // public string IsLearned { get; set; }
    // public string IsMastered { get; set; }
    // public string IsDeleted { get; set; }
    // public string CreatedAt { get; set; }
    // public string UpdatedAt { get; set; }
    // public string DeletedAt { get; set; }
    public string ChineseMeaning { get; set; }
    public bool IsLearning { get; set; }

    public int NumTyped { get; set; }
    public int NumCorrect { get; set; }
}

public class DataController : MonoBehaviour
{
    public SQLiteConnection connection;
    private string dbPath = Application.streamingAssetsPath + "/Dictonary.db";

    private void Awake()
    {
        connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    private void OnDestroy()
    {
        connection.Close();
    }

    public List<EnglishWord> Query()
    {
        var table = connection.Table<EnglishWord>();
        return table.ToList();
    }

    public void UpdateEnglishWord(EnglishWord word)
    {
        connection.Update(word);
    }
}


