using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private GameObject controller;
    public Local stringSet;
    private int counter;
    private bool dialogisOn;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        dialogisOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogisOn)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                counter += 1;
                if(counter >= stringSet.dialogue.Length)
                {
                    controller.GetComponent<Controller>().DialogPanel.SetActive(false);
                    controller.GetComponent<Controller>().inDialog = false;
                    controller.GetComponent<Controller>().MainPanel.SetActive(true);
                    dialogisOn = false;
                    return;

                }
                controller.GetComponent<Controller>().DialogPanel.GetComponentInChildren<Text>().text = stringSet.dialogue[counter];
            }
        }
    }
    public void startDialog()
    {

        counter = 0;
        Debug.Log("no");
        controller.GetComponent<Controller>().DialogPanel.SetActive(true);
        controller.GetComponent<Controller>().DialogPanel.GetComponentInChildren<Text>().text = stringSet.dialogue[counter];
        dialogisOn = true;
    }
}
