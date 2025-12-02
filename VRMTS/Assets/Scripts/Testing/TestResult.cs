using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TestResult
{
    public string userId;
    public string labName;
    public string date;
    public int totalQuestions;
    public int correct;
    public int incorrect;

    public List<string> gainedTags = new();
    public List<string> lostTags = new();
}
