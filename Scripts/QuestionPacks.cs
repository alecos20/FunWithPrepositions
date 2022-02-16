using System.Collections.Generic;

public class QuestionPacks {
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

    public QuestionPacks(string wordIndexes, string descriptionIndexes, string hsTimerSpeed, string timerSpeed, string userName,
        string date, float accuracy, float score, float lastAcc, float lastScore)
    {
        this.wordIndexes = wordIndexes;
        this.descriptionIndexes = descriptionIndexes;
        this.hsTimerSpeed = hsTimerSpeed;
        this.timerSpeed = timerSpeed;
        this.userName = userName;
        this.date = date;
        this.accuracy = accuracy;
        this.score = score;
        this.lastAcc = lastAcc;
        this.lastScore = lastScore;
    }

    public QuestionPacks()
    {

    }

    public void setWordIndexes(string wordIndexes)
    {
        this.wordIndexes = wordIndexes;
    }

    public void setDescriptionIndexes(string descriptionIndexes)
    {
        this.descriptionIndexes = descriptionIndexes;
    }

    public string getWordIndexes()
    {
        return wordIndexes;
    }

    public string getDescriptionIndexes()
    {
        return descriptionIndexes;
    }

    public void setDate(string date)
    {
        this.date = date;
    }

    public void setAccuracy(float accuracy)
    {
        this.accuracy = accuracy;
    }

    public void setScore(float score)
    {
        this.score = score;
    }

    public string getDate()
    {
        return date;
    }

    public float getAccuracy()
    {
        return accuracy;
    }

    public float getScore()
    {
        return score;
    }

    public float getLastAccuracy()
    {
        return lastAcc;
    }

    public void setLastAccuracy(float lastAcc)
    {
        this.lastAcc = lastAcc;
    }

    public float getLastScore()
    {
        return lastScore;
    }

    public void setTimerSpeed(string timerSpeed)
    {
        this.timerSpeed = timerSpeed;
    }

    public string getTimerSpeed()
    {
        return timerSpeed;
    }

    public void setHSTimerSpeed(string hsTimerSpeed)
    {
        this.hsTimerSpeed = hsTimerSpeed;
    }

    public string getHSTimerSpeed()
    {
        return hsTimerSpeed;
    }

    public void setUserName(string userName)
    {
        this.userName = userName;
    }

    public string getUserName()
    {
        return userName;
    }

    public void setLastScore(float lastScore)
    {
        this.lastScore = lastScore;
    }

    public void Save()
    {
        SaveLoadManager.saveQuestionPack(this);
    }

    public void SaveGenerated()
    {
        SaveLoadManager.saveGeneratedQP(this);
    }

    public QuestionPacks Load()
    {
        return SaveLoadManager.loadQuestionPack();
    }

    public List<QuestionPacks> LoadAll()
    {
        return SaveLoadManager.loadAllQuestionPacks();
    }

    public string[] FileNames()
    {
        return SaveLoadManager.loadAllFileNames();
    }
}
