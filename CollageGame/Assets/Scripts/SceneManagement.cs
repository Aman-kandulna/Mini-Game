using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
  public void GoToScene()
    {
        SceneManager.LoadScene("TestScene");
    }
    public void ExitApp()
    {
        Application.Quit();
    }
}
