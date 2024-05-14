using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuOptions : MonoBehaviour
{
    public void Retour()
    {
        SceneManager.LoadScene("menu");
    }
}
