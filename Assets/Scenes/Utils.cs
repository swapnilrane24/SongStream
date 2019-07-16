using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestionStruct;
using System;

public class Utils : MonoBehaviour
{
    Question[] questions;
    public StreamMusic streamMusic;

    private void Start()
    {
        StartCoroutine(streamMusic.LoadJson(GetQuestions));
        //Utils.EventAsync(new GameEvent.ActiveChallenges(GetQestions);
    }

    private void GetQuestions(Question[] questions)
    {
        this.questions = questions; 
    }

}