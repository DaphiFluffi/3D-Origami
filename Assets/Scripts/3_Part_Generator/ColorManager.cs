using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private int counter;
   public  void Ping(Color colorWeGot)
    {
        counter++;
        Debug.Log(counter);
        Debug.Log(colorWeGot);
    }
}
