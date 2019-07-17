using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestionStruct;
using System;

public class Utils : MonoBehaviour
{
    Question[] questions;
    public StreamMusic streamMusic;

    public event Action<Action<Question[]>> getQuestion;
    bool pressed = false;

    private void Start()
    {
        //StartCoroutine(streamMusic.LoadJson(GetQuestions));
        //Utils.EventAsync(new GameEvent.ActiveChallenges(GetQestions);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !pressed)
        {
            pressed = true;
            getQuestion.Invoke(GetQuestions);
        }
    }

    private void GetQuestions(Question[] questions)
    {
        this.questions = questions; 
    }

}