using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysOnCamera : MonoBehaviour
{
    void Start()
    {
        // prevents phone screen from tutning off 
        // https://answers.unity.com/questions/46204/stop-mobile-screens-turning-off.html
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
