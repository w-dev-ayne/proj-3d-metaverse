using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class APIManager : MonoBehaviour
{
    public LeaderBoard leaderBoardData;
    public QuizData quizData;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GetLeaderBoard();
        GetQuizzes();
    }

    private void GetLeaderBoard()
    {
        
        leaderBoardData = LoadSampleJsonFile<LeaderBoard>($"{Application.dataPath}/Resources/TestData", "UserScores");
        
    }

    private void GetQuizzes()
    {
        quizData = LoadSampleJsonFile<QuizData>($"{Application.dataPath}/Resources/TestData", "Quizzes");
        QuizManager.GenerateSampleQuizzes();
    }

    public T LoadSampleJsonFile<T>(string loadPath, string fileName)
    {
#if !UNITY_EDITOR
        string path = $"TestData/{fileName}";
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        string jsonData = textAsset.text;
#else
        string path = $"{loadPath}/{fileName}.json";
        string jsonData = File.ReadAllText(path);
#endif
        return JsonUtility.FromJson<T>(jsonData);
    }
}

[System.Serializable]
public class LeaderBoard
{
    public List<UserScore> userScores;

    public void Print()
    {
        Debug.Log(userScores);
    }
}

[System.Serializable]
public class UserScore
{
    public int rank;
    public string name;
    public int point;

    public void Print()
    {
        Debug.Log($"Order : {this.rank} \nName : {this.name} \nPoint : {this.point}");
    }
}

[System.Serializable]
public class QuizData
{
    public List<Quiz> quizzes;
}

[System.Serializable]
public class Quiz
{
    public int index;
    public string title;
    public string info;
    public string quiz;
    public string answer;
    public string type;
    public Define.QuizAnswer realAnswer;
    public bool isSolved = false;
    public bool isCoolTime = false;
    public int coolTime = 60;
    public Define.QuizType realType;
    
    public void SetRealData()
    {
        realAnswer = (answer == "O" || answer == "o") ? Define.QuizAnswer.O : Define.QuizAnswer.X;
        realType = (type == "Crypto") ? Define.QuizType.Crypto : Define.QuizType.DNA;
    }
    
    public bool IsCorrect(Define.QuizAnswer inputAnswer)
    {
        return this.realAnswer == inputAnswer;
    }

    public void Solved()
    {
        this.isSolved = true;
        QuizManager.currentQuiz = null;
        Managers.UI.FindPopup<UI_Quiz>().ClosePopupUI();
    }
}