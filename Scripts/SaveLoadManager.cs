using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager {

    public static List<QuestionPacks> loadAllQuestionPacks()
    {
        List<QuestionPacks> allQPs = new List<QuestionPacks>();
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.qp");
       
        for(int i = 0; i < files.Length; i++)
        {
            if (File.Exists(files[i]))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(files[i], FileMode.Open);
                QuestionPackData qpD = (QuestionPackData)bf.Deserialize(stream);
                allQPs.Add(new QuestionPacks(qpD.wordIndexes, qpD.descriptionIndexes, qpD.hsTimerSpeed ,qpD.timerSpeed, qpD.userName, qpD.date, qpD.accuracy, qpD.score, qpD.lastAcc, qpD.lastScore));
                stream.Close();
            }
        }
        
        return allQPs;
    }

    public static string[] loadAllFileNames()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.qp");

        return files;
    }

    public static void saveQuestionPack(QuestionPacks qp)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream;
        
        if (File.Exists(PlayerPrefs.GetString("ChosenPack")))
        {
            stream = new FileStream(PlayerPrefs.GetString("ChosenPack"), FileMode.Truncate);
        } else 
        {
            stream = new FileStream(Application.persistentDataPath + "/"
                + DateTime.Now.ToString("MM_dd_yyyy") + ".qp", FileMode.Create);
        }

        QuestionPackData data = new QuestionPackData(qp);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static void saveGeneratedQP(QuestionPacks qp)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream;

        stream = new FileStream(Application.persistentDataPath + "/"
                + qp.date + DateTime.Now.ToString("HH_mm_ss") + ".qp", FileMode.Create);
        Debug.Log(stream.Name);

        QuestionPackData data = new QuestionPackData(qp);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static QuestionPacks loadQuestionPack()
    {
        if (File.Exists(Application.persistentDataPath + "/" + DateTime.Now.ToString("MM_dd_yyyy") + ".qp"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + DateTime.Now.ToString("MM_dd_yyyy") + ".qp", FileMode.Open);

            QuestionPackData qp = (QuestionPackData)bf.Deserialize(stream);
            stream.Close();
            return new QuestionPacks(qp.wordIndexes, qp.descriptionIndexes, qp.hsTimerSpeed, qp.timerSpeed, qp.userName, qp.date, qp.accuracy, qp.score, qp.lastAcc, qp.lastScore);
        }
        else
        {
            return null;
        }
    }

    public static QuestionPacks loadChosenQP(string fileName)
    {
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(fileName, FileMode.Open);

            QuestionPackData qp = (QuestionPackData)bf.Deserialize(stream);
            stream.Close();
            return new QuestionPacks(qp.wordIndexes, qp.descriptionIndexes, qp.hsTimerSpeed, qp.timerSpeed, qp.userName, qp.date, qp.accuracy, qp.score, qp.lastAcc, qp.lastScore);
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class QuestionPackData
{
    public string wordIndexes;
    public string descriptionIndexes;
    public string hsTimerSpeed;
    public string timerSpeed;
    public string userName;
    public string date;
    public float accuracy;
    public float score;
    public float lastAcc;
    public float lastScore;

    public QuestionPackData(QuestionPacks qp)
    {
        wordIndexes = qp.wordIndexes;
        descriptionIndexes = qp.descriptionIndexes;
        hsTimerSpeed = qp.hsTimerSpeed;
        timerSpeed = qp.timerSpeed;
        userName = qp.userName;
        date = qp.date;
        accuracy = qp.accuracy;
        score = qp.score;
        lastAcc = qp.lastAcc;
        lastScore = qp.lastScore;
    }
}