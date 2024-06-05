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
        SampleManager._instance.onStateChange += state => OnStateChange(state);
        OnStateChange(SampleManager._instance.gameState);
    }

    private void OnStateChange(GameState state)
    {
        if (state != GameState.GeneralFeedbackGood && state != GameState.GeneralFeedbackBad)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        perc = SampleManager._instance.classManagementScore / 2 > 0 ? SampleManager._instance.classManagementScore : 0;
        bool passed = true;
        text.text = "";
        if (avgSightTime > 2.5f)
        {
            if (state == GameState.GeneralFeedbackGood)
            {
                passed = true;
            }
        }
        else if (state == GameState.GeneralFeedbackBad)
        {
        }
        if (SampleManager._instance.mode == GameState.Learning)
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
                passed = true;
            }
        }
        else if (state == GameState.GeneralFeedbackBad)
        {
        }
        if (useOfObjTime > 30)
        {
            if (state == GameState.GeneralFeedbackGood)
            {
                passed = true;
            }
        }
        else if (state == GameState.GeneralFeedbackBad)
        {
        }
        if (SampleManager._instance.mode == GameState.Crafting)
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

        if (GameState.GeneralFeedbackGood == state && !passed) SampleManager._instance.ChangeGameMode();

    }
}
