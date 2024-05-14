using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

   public void Jouer ()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit ()
    {
        Application.Quit();
    }
    public void Options ()
    {
        SceneManager.LoadScene("Option menu");
    }
}
