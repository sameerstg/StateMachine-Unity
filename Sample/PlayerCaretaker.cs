using System;
using UnityEngine;

public class PlayerCaretaker : MonoBehaviour
{

    public VoiceDetector VoiceDetector;
    private void Awake()
    {
        VoiceDetector = GetComponent<VoiceDetector>();  
    }
}
