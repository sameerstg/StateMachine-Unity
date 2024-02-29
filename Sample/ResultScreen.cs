using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    public float avgSightTime, avgUnfocusAmount, useOfObjTime;
    public int perc;
    public float leastTimesLearnedStudent;
    public TextMeshProUGUI text;
    private void Start()
    {
        GameManager._instance.onStateChange += state => OnStateChange(state);
        OnStateChange(GameManager._instance.gameState);
    }

    private void OnStateChange(GameState state)
    {
        if (state != GameState.GeneralFeedbackGood && state != GameState.GeneralFeedbackBad)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        List<Student> students = GameManager._instance.sittingManager.instantiatedStudents.Select(x => x.GetComponent<Student>()).ToList();
        avgSightTime = (float)TimeSpan.FromSeconds(students.Average(x => x.sightInteractable.totalSightTime)).TotalSeconds;
        avgUnfocusAmount = (float)Math.Round(students.Average(x => x.timesUnfocussed), 2);
        useOfObjTime = TimeSpan.FromSeconds(GameManager._instance.storyObjects.Average(x => x.timeGrabbed)).Seconds;
        perc = GameManager._instance.classManagementScore / 2 > 0 ? GameManager._instance.classManagementScore : 0;
        leastTimesLearnedStudent = GameManager._instance.sittingManager.instantiatedStudents.Min(x => x.timesLearned);
        bool passed = true;
        text.text = "";
        if (avgSightTime > 2.5f)
        {
            if (state == GameState.GeneralFeedbackGood)
            {
                text.text += $"Average Sight Time : {TimeSpan.FromSeconds(students.Average(x => x.sightInteractable.totalSightTime)).TotalSeconds} sec \n";
                passed = true;
            }
        }
        else if (state == GameState.GeneralFeedbackBad)
        {
            text.text += $"Average Sight Time : {TimeSpan.FromSeconds(students.Average(x => x.sightInteractable.totalSightTime)).TotalSeconds} sec \n";
        }
        if(GameManager._instance.mode == GameState.Learning)
        {
            if (leastTimesLearnedStudent >= 3)
            {
                if (state == GameState.GeneralFeedbackGood)
                {
                    text.text += $"Student with least participation : {leastTimesLearnedStudent} times \n";
                    passed = true;
                }
            }
            else if (state == GameState.GeneralFeedbackBad)
            {
                text.text += $"Student with least participation : {leastTimesLearnedStudent} times \n";
            }
        }
       
        if (avgUnfocusAmount < 3)
        {
            if (state == GameState.GeneralFeedbackGood)
            {
                text.text += $"Average Unfocussed Amount : {Math.Round(students.Average(x => x.timesUnfocussed), 2)}\n";
                passed = true;
            }
        }
        else if (state == GameState.GeneralFeedbackBad)
        {
            text.text += $"Average Unfocussed Amount : {Math.Round(students.Average(x => x.timesUnfocussed), 2)}\n";
        }
        if (useOfObjTime > 30)
        {
            if (state == GameState.GeneralFeedbackGood)
            {
                passed = true;
                text.text += $"Use of Objects Average Time : {TimeSpan.FromSeconds(GameManager._instance.storyObjects.Average(x => x.timeGrabbed)).Seconds} sec\n";
            }
        }
        else if (state == GameState.GeneralFeedbackBad)
        {
            text.text += $"Use of Objects Average Time : {TimeSpan.FromSeconds(GameManager._instance.storyObjects.Average(x => x.timeGrabbed)).Seconds} sec\n";
        }
        if(GameManager._instance.mode == GameState.Crafting)
        {
            if (perc >= 60)
            {
                if (state == GameState.GeneralFeedbackGood)
                {
                    text.text += $"Class Management Score : {perc}%";
                    passed = true;
                }
            }
            else if (state == GameState.GeneralFeedbackBad)
            {
                text.text += $"Class Management Score : {perc}%";
            }
        }
       
        if (GameState.GeneralFeedbackGood == state && !passed) GameManager._instance.ChangeGameMode();

    }
}
