using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMech : MonoBehaviour
{
    public enum difficulty { low, med, hard }
    public difficulty dif;
    public enum gameMechMiniGame { gameMech1, gameMech2, gameMech3 }
    public gameMechMiniGame gmMechMiniGame;
    
    public bool isObjectMech;
    public Transform camPos;
    public Transform lookAtPos;
    private GameObject controller;


    GameObject[] containers;
    GameObject[] learnedobj;

    private Vector3 mcCaminit;
    private Transform mcCamPos;
    private bool isCamMoving;
    private bool isCamMovingNormalize;


    private List<NonObjectWord> learnedWords = new List<NonObjectWord>();
    private List<Word> learnedObjects = new List<Word>();

    private GameObject gm1;
    private GameObject gm2;
    private GameObject gm3;

    public AudioClip succes;
    public AudioClip fail;

    /*
     * 3 oyun mekanizmasý 
     * 1 karþýlýklý selection zorluða göre 3-4-5
     * 2 kelime tamamlama tek kelime
     * 3 hafýza oyunu zorluða göre 7-8-9
     */
    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        switch (gmMechMiniGame)
        {
            case gameMechMiniGame.gameMech1:
                gm1 = gameObject.GetComponentInChildren<GM1>().gameObject;
                break;
            case gameMechMiniGame.gameMech2:
                gm2 = gameObject.GetComponentInChildren<GM2>().gameObject;
                break;
            case gameMechMiniGame.gameMech3:
                gm3 = gameObject.GetComponentInChildren<GM3>().gameObject;
                break;
            default:
                break;
        }
        isCamMoving = false;
        isCamMovingNormalize = false;

    }
    void Update()
    {
        if (isCamMoving)
        {
            var step = 2 * Time.deltaTime;
            mcCamPos.position = Vector3.MoveTowards(mcCamPos.position, camPos.position, step);
            mcCamPos.LookAt(lookAtPos);
            if (camPos.position == mcCamPos.position)
            {
                isCamMoving = false;
                //minigameStart
                if (gmMechMiniGame == gameMechMiniGame.gameMech1)
                {
                    startMiniGame1();
                }
                else if (gmMechMiniGame == gameMechMiniGame.gameMech2)
                {
                    startMiniGame2();
                }
                else if (gmMechMiniGame == gameMechMiniGame.gameMech3)
                {

                }
            }
        }
        else if (isCamMovingNormalize)
        {
            var step = 2 * Time.deltaTime;
            mcCamPos.position = Vector3.MoveTowards(mcCamPos.position, mcCaminit, step);
            if (mcCaminit == mcCamPos.position)
            {
                isCamMovingNormalize = false;
            }
        }
    }


    public void gameMechStart(GameObject mcCam)
    {
        mcCamPos = mcCam.transform;
        bool stopGameMech;

        containers = GameObject.FindGameObjectsWithTag("Container");
        for (int c = 0; c < containers.Length; c++)
        {
            for (int i = 0; i < containers[c].GetComponent<ContainerManager>().containsWord; i++)
            {
                if (containers[c].GetComponent<ContainerManager>().nonObjectWordsdb[i].isLearned)
                {
                    NonObjectWord tempNOW = containers[c].GetComponent<ContainerManager>().nonObjectWordsdb[i];
                    if (!learnedWords.Contains(tempNOW))
                    {
                        learnedWords.Add(tempNOW);
                    }
                }
            }
        }
        learnedobj = GameObject.FindGameObjectsWithTag("LearnedObject");
        for (int c = 0; c < learnedobj.Length; c++)
        {
            Debug.Log(learnedObjects);
            if (learnedobj[c].GetComponent<ObjectController>().word.isLearned)
            {
                Word tempWord = learnedobj[c].GetComponent<ObjectController>().word;
                if (!learnedObjects.Contains(tempWord))
                {
                    learnedObjects.Add(tempWord);
                }
            }
        }
        switch (gmMechMiniGame)
        {
            case gameMechMiniGame.gameMech1:
                stopGameMech = checkMechCanWork(3);
                break;
            case gameMechMiniGame.gameMech2:
                stopGameMech = checkMechCanWork(1);
                break;
            case gameMechMiniGame.gameMech3:
                stopGameMech = checkMechCanWork(8);
                break;
            default:
                Debug.Log("Bug has been accured");
                stopGameMech = true;
                break;
        }
        Debug.Log(stopGameMech);
        if (stopGameMech)
        {
            stopgameMech("Daha fazla kelime öðren",true);
        }
        else
        {
            mcCaminit = mcCamPos.position;
            isCamMovingNormalize = false;
            isCamMoving = true;
        }
    }
    private void startMiniGame1()
    {
        int maxElem = 3;
        controller.GetComponent<Controller>().infoPause.GetComponentInChildren<Text>().text = "Mini Oyunu durdurmak için ESC'e basýn";
        controller.GetComponent<Controller>().infoCodex.GetComponentInChildren<Text>().text = "Ýþleminizi kontrol ettirmek için ENTER'e basýn";
        if (!isObjectMech)
        {
            List<NonObjectWord> wordElem = new List<NonObjectWord>();
            List<NonObjectWord> allElem = learnedWords;
            switch (dif)
            {
                case difficulty.low:
                    maxElem = 3;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedWords[count]);
                        learnedWords.RemoveAt(count);
                    }
                    break;
                case difficulty.med:
                    maxElem = 4;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedWords[count]);
                        learnedWords.RemoveAt(count);
                    }
                    break;
                case difficulty.hard:
                    maxElem = 5;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedWords[count]);
                        learnedWords.RemoveAt(count);
                    }
                    break;
                default:
                    Debug.Log("Bug has been accured");
                    break;
            }
            string[] left = new string[maxElem];
            string[] right = new string[maxElem];
            List<int> leftNumber = new List<int>();
            List<int> rightNumber = new List<int>();
            for (int x = 0; x < maxElem; x++)
            {
                leftNumber.Add(x);
                rightNumber.Add(x);
            }
            for (int i = 0; i < wordElem.Count; i++)
            {
                //left
                int index = Random.Range(0, leftNumber.Count);
                left[leftNumber[index]] = wordElem[i].nameEN;
                leftNumber.RemoveAt(index);
                //right
                index = Random.Range(0, rightNumber.Count);
                right[rightNumber[index]] = wordElem[i].nameTR;
                rightNumber.RemoveAt(index);
            }
            gm1.GetComponentInChildren<GM1>().miniGameStartNonObject(left, right, wordElem);
        }
        else
        {
            List<Word> wordElem = new List<Word>();
            List<Word> allElem = learnedObjects;
            switch (dif)
            {
                case difficulty.low:
                    maxElem = 3;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedObjects[count]);
                        learnedObjects.RemoveAt(count);
                    }
                    break;
                case difficulty.med:
                    maxElem = 4;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedObjects[count]);
                        learnedObjects.RemoveAt(count);
                    }
                    break;
                case difficulty.hard:
                    maxElem = 5;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedObjects[count]);
                        learnedObjects.RemoveAt(count);
                    }
                    break;
                default:
                    Debug.Log("Bug has been accured");
                    break;
            }
            string[] left = new string[maxElem];
            string[] right = new string[maxElem];
            List<int> leftNumber = new List<int>();
            List<int> rightNumber = new List<int>();
            for (int x = 0; x < maxElem; x++)
            {
                leftNumber.Add(x);
                rightNumber.Add(x);
            }
            for (int i = 0; i < wordElem.Count; i++)
            {
                //left
                int index = Random.Range(0, leftNumber.Count);
                left[leftNumber[index]] = wordElem[i].wordSTR_EN;
                leftNumber.RemoveAt(index);
                //right
                index = Random.Range(0, rightNumber.Count);
                right[rightNumber[index]] = wordElem[i].wordSTR_TR;
                rightNumber.RemoveAt(index);
            }
            gm1.GetComponentInChildren<GM1>().miniGameStart(left,right,wordElem);
        }
    }
    private void startMiniGame2()
    {
        int maxElem;
        controller.GetComponent<Controller>().infoPause.GetComponentInChildren<Text>().text = "Mini Oyunu durdurmak için ESC'e basýn";
        controller.GetComponent<Controller>().infoCodex.GetComponentInChildren<Text>().text = "Ýþleminizi kontrol ettirmek için ENTER'e basýn";
        if(!isObjectMech)
        {
            List<NonObjectWord> wordElem = new List<NonObjectWord>();
            List<NonObjectWord> allElem = learnedWords;
            switch (dif)
            {
                case difficulty.low:
                    maxElem = 1;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedWords[count]);
                        learnedWords.RemoveAt(count);
                    }
                    break;
                case difficulty.med:
                    maxElem = 2;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedWords[count]);
                        learnedWords.RemoveAt(count);
                    }
                    break;
                case difficulty.hard:
                    maxElem = 3;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedWords[count]);
                        learnedWords.RemoveAt(count);
                    }
                    break;
                default:
                    Debug.Log("Bug has been accured");
                    break;
            }
            gm2.GetComponentInChildren<GM2>().miniGameStartNonObject(wordElem);
        }
        else
        {
            List<Word> wordElem = new List<Word>();
            List<Word> allElem = learnedObjects;
            switch (dif)
            {
                case difficulty.low:
                    maxElem = 1;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedObjects[count]);
                        learnedObjects.RemoveAt(count);
                    }
                    break;
                case difficulty.med:
                    maxElem = 2;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedObjects[count]);
                        learnedObjects.RemoveAt(count);
                    }
                    break;
                case difficulty.hard:
                    maxElem = 3;
                    for (int i = 0; i < maxElem; i++)
                    {
                        int count = Random.Range(0, allElem.Count);
                        wordElem.Add(learnedObjects[count]);
                        learnedObjects.RemoveAt(count);
                    }
                    break;
                default:
                    Debug.Log("Bug has been accured");
                    break;
            }
            gm2.GetComponentInChildren<GM2>().miniGameStart(wordElem);
        }
    }
    private bool checkMechCanWork(int mincount)
    {
        switch (dif)
        {
            case difficulty.low:
                if (!isObjectMech)
                {
                    if (learnedWords.Count >= mincount)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (learnedObjects.Count >= mincount)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            case difficulty.med:
                if (!isObjectMech)
                {
                    if (learnedWords.Count >= mincount + 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (learnedObjects.Count >= mincount + 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            case difficulty.hard:
                if (!isObjectMech)
                {
                    if (learnedWords.Count >= mincount + 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    if (learnedObjects.Count >= mincount + 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            default:
                Debug.Log("There is a bug accured");
                return true;
        }
    }
    public void stopgameMech(string reason,bool notEnough)
    {
        if (notEnough)
        {
            StartCoroutine(camNormalF(reason));
            return;
        }
        if (!isCamMoving)
        {
            switch (gmMechMiniGame)
            {
                case gameMechMiniGame.gameMech1:
                    gm1.GetComponent<GM1>().playerInterrupt();
                    break;
                case gameMechMiniGame.gameMech2:
                    gm2.GetComponent<GM2>().playerInterrupt();
                    break;
                case gameMechMiniGame.gameMech3:
                    break;
                default:
                    break;
            }
            isCamMovingNormalize = true;
            StartCoroutine(camNormalF(reason));
        }
        else
        {
            return;
        }
    }
    IEnumerator camNormalF(string str)
    {
        while (isCamMovingNormalize)
        {
            yield return new WaitForSeconds(0f);
        }
        Controller controllerComp = controller.GetComponent<Controller>();

        GameObject temptext = GameObject.Instantiate(controllerComp.popUpTextPrefab.gameObject, controllerComp.MainPanel.transform);
        temptext.GetComponent<Text>().text = str;
        StartCoroutine(controllerComp.popUpAnim(0.05f, temptext.GetComponent<Text>()));

        controller.GetComponent<Controller>().timerText.transform.parent.parent.gameObject.SetActive(false);


        controller.GetComponent<Controller>().infoPause.GetComponentInChildren<Text>().text = "Oyunu durdurmak için '" + controller.GetComponent<Controller>().pauseButton.ToString() + "'e basýn";
        controller.GetComponent<Controller>().infoCodex.GetComponentInChildren<Text>().text = "Codexi açmak için '" + controller.GetComponent<Controller>().codexButton.ToString() + "'e basýn";
        controller.GetComponent<Controller>().inGameMech = false;
    }
}
