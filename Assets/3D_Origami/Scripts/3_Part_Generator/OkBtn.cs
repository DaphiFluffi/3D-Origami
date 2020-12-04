using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OkBtn : MonoBehaviour
{
   [SerializeField] private GameObject errorPanel = default;

   public void HideErrorPanel()
   {
      errorPanel.gameObject.SetActive(false);
   }
}
