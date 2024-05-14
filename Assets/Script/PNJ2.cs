using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PNJ2 : MonoBehaviour
{
    [SerializeField]
    string[] sentences;

    [SerializeField]
    string characterName;
    int index;
    bool isOndial, canDial;

    HUDmanager manager => HUDmanager.instance;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canDial)
        {
            StartDialogue();
            manager.continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
            manager.continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLine(); });
        }
    }

    void TypingText(string[] sentence)
    {
        manager.DisplayName.text = "";
        manager.displayText.text = "";

        manager.DisplayName.text = characterName;
        manager.displayText.text = sentence[index];

        if (manager.displayText.text == sentence[index])
        {
            manager.continueButton.SetActive(true);
        }


    }

    public void StartDialogue()
    {
        manager.DialogueHolder.SetActive(true);
        isOndial = true;
        TypingText(sentences);

    }
    public void NextLine()
    {
        manager.continueButton.SetActive(false);

        if (isOndial && index < sentences.Length - 1)
        {
            index++;
            manager.displayText.text = "";
            TypingText(sentences);
        }
        else if (isOndial && index == sentences.Length - 1)
        {
            isOndial = false;
            index = 0;
            manager.displayText.text = "";
            manager.DialogueHolder.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canDial = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canDial = false;
        }
    }
  }
