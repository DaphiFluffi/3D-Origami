using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
   public void OnPart1Click()
   {
      SceneManager.LoadScene(1);
   }
   
   public void OnPart2Click()
   {
      SceneManager.LoadScene(2);
   }
   
   public void OnPart3Click()
   {
      SceneManager.LoadScene(3);
   }

   public void BackToMenu()
   {
      SceneManager.LoadScene(0);
   }

   public void Quit()
   {
      Application.Quit();
   }
}
