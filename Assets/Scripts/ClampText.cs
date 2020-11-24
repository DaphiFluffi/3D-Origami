using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// https://www.youtube.com/watch?v=0bvDmqqMXcA
public class ClampText : MonoBehaviour
{
   public Button nameLable;
       
    // Update is called once per frame
    void Update () {
           Vector3 namePose = Camera.main.WorldToScreenPoint(this.transform.position);
           nameLable.transform.position = namePose;
    }
   
}
