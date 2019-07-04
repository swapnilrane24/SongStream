using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionStruct
{
    public int status;
    public Question[] questions;

    [System.Serializable]
    public struct Question
    {
        public string question;
        [HideInInspector]
        public string options;
        public string answer;
        public string songurl;
        public string[] option;
    }
}