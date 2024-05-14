using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDmanager : MonoBehaviour
{
    public static HUDmanager instance;


    public GameObject DialogueHolder, continueButton;
    public TextMeshProUGUI DisplayName, displayText;


    public void Awake()
    {
        instance = this;
    }
}