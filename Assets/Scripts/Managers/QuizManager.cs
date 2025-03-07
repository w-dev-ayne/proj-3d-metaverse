using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class QuizManager
{
    public static Quiz currentQuiz;
    public static Dictionary<int, Quiz> quizzes = new Dictionary<int, Quiz>();

    public static void GenerateSampleQuizzes()
    {
        int count = 1;
        foreach (Quiz quiz in Managers.API.quizData.quizzes)
        {
            quizzes[count] = quiz;
            count++;
            quiz.SetRealData();
        }
    }

    public static void GenerateQuiz(Quiz quiz)
    {
        quizzes[quiz.index] = quiz;
    }

    public static void ShowQuiz(int quizNum)
    {
        currentQuiz = quizzes[quizNum];

        if (currentQuiz.isCoolTime)
        {
            InstructionManager.CurrentInstruction = $"{currentQuiz.coolTime.ToString()}초 뒤에 다시 시도해주세요.";
            Managers.UI.ShowPopupUI<UI_Info>();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_Quiz>();    
        }
    }
}
