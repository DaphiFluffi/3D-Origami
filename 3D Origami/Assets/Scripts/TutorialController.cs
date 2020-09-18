using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] folds = new GameObject[9];

    private int _index;

    void Awake()
    {
        folds[0].SetActive(true);
        _index = 0;
    }
    
    public void NextAnimation()
    {
        if (_index != 8)
        {
            folds[_index].SetActive(false);
            folds[_index + 1].SetActive(true);
            _index += 1;
        }
    }
    
    public void PreviousAnimation()
    {
        if (_index != 0)
        {
            folds[_index].SetActive(false);
            folds[_index - 1].SetActive(true);
            _index -= 1;
        }
    }
}
