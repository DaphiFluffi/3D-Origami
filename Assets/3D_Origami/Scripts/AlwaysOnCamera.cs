using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysOnCamera : MonoBehaviour
{
    void Start()
    {
        // prevents phone screen from turning off 
        // http://answers.unity.com/answers/247524/view.html
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
