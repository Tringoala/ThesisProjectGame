using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexController : MonoBehaviour
{
    [HideInInspector]
    public Word wordThatinGame;
    public GameObject codexElement;
    public GameObject codexElementONTO;
    public Sprite notLearnedObjectImage;
    public Button btnModeChanger;

    GameObject[] learnedobjcsts;
    GameObject[] learnableobjcsts;
    GameObject[] containers;


    List<GameObject> elemList;
    List<Word> words = new List<Word>();
    List<NonObjectWord> wordsONTO = new List<NonObjectWord>();

    int elemCount;
    bool modObject;
    
    private void firstStart()
    {
        elemCount = 0;
        modObject = false;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        learnableobjcsts = GameObject.FindGameObjectsWithTag("LearnableObject");
        learnedobjcsts = GameObject.FindGameObjectsWithTag("LearnedObject");

        containers = GameObject.FindGameObjectsWithTag("Container");
        Debug.Log(learnedobjcsts.Length);
        //wordObject
        for (int i = 0; i < learnedobjcsts.Length;)
        {
            Word tempWord = learnedobjcsts[i].GetComponent<ObjectController>().word;
            if (i != 0)
            {
                if (words.Contains(tempWord))
                {
                    i += 1;
                    continue;
                }
            }
            words.Add(tempWord);
            i += 1;
        }
        for (int i = 0; i < learnableobjcsts.Length;)
        {
            Word tempWord = learnableobjcsts[i].GetComponent<ObjectController>().word;
            if (i != 0)
            {
                if (words.Contains(tempWord))
                {
                    i += 1;
                    continue;
                }
            }
            words.Add(tempWord);
            i += 1;
        }
        //wordOnto
        for (int i = 0; i < containers.Length; i++)
        {
            for(int j = 0; j < containers[i].GetComponent<ContainerManager>().containsWord;)
            {
                Debug.Log(containers[i].name);
                NonObjectWord tempWord = containers[i].GetComponent<ContainerManager>().nonObjectWordsdb[j];
                if (j != 0)
                {
                    if (wordsONTO.Contains(tempWord))
                    {
                        j += 1;
                        continue;
                    }
                }
                wordsONTO.Add(tempWord);
                j += 1;
            }
        }
        watch.Stop();
        Debug.Log(watch.ElapsedMilliseconds.ToString());
        spawnCodexElementfromObject();
    }

    public void codexOpenConnector()
    {
        Time.timeScale = 0f;
        firstStart();
    }
    private void spawnCodexElementfromObject()
    {
        GameObject tempElement;
        for (int j = 0; j < words.Count; j++)
        {
            tempElement = Instantiate(codexElement, gameObject.transform);
            tempElement.GetComponent<Codex>().codexNameEN = words[j].wordSTR_EN;
            tempElement.GetComponent<Codex>().codexNameEN = words[j].wordSTR_EN;
            tempElement.GetComponent<Codex>().codexNameTR = words[j].wordSTR_TR;
            tempElement.GetComponent<Codex>().codexImage = words[j].word_UI_Image;
            if (!words[j].isLearned)
            {
                tempElement.GetComponent<Codex>().codexImage = notLearnedObjectImage;
                tempElement.GetComponent<Codex>().codexNameTR = "???";
                tempElement.GetComponent<Codex>().codexNameEN = "???";
            }
            elemCount += 1;
        }
    }
    private void spawnCodexElementfromOnto()
    {
        GameObject tempElement;
        for (int j = 0; j < wordsONTO.Count; j++)
        {
            tempElement = Instantiate(codexElementONTO, gameObject.transform);
            tempElement.GetComponent<CodexForOnto>().codexNameEN = wordsONTO[j].nameEN;
            tempElement.GetComponent<CodexForOnto>().codexNameTR = wordsONTO[j].nameTR;
            tempElement.GetComponent<CodexForOnto>().codexDesc = wordsONTO[j].desc;
            if (!wordsONTO[j].isLearned)
            {
                tempElement.GetComponent<CodexForOnto>().codexDesc = "???";
                tempElement.GetComponent<CodexForOnto>().codexNameTR = "???";
                tempElement.GetComponent<CodexForOnto>().codexNameEN = "???";
            }
            elemCount += 1;
        }
    }

    public void changeMode()
    {
        for (int i = 0; i < elemCount; i++)
        {
            DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }
        elemCount = 0;
        modObject = !modObject;
        if (modObject)
        {
            btnModeChanger.gameObject.GetComponentInChildren<Text>().text = "Objeler";
            spawnCodexElementfromOnto();
        }
        else
        {
            btnModeChanger.gameObject.GetComponentInChildren<Text>().text = "Kelimeler";
            spawnCodexElementfromObject();
        }
    }

    public void codexClosed()
    {
        for (int i = 0; i < gameObject.transform.childCount;)
        {
            DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }
        words.Clear();
        wordsONTO.Clear();
        Time.timeScale = 1f;
    }
}
