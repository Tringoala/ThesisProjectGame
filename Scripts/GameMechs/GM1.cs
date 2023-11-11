using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM1 : MonoBehaviour
{
    private Controller controller;

    public GameMech gameMech;
    public GameObject textInfoL;
    public GameObject textInfoR;
    public GameObject[] textL;
    public GameObject[] sprtieL;
    public GameObject[] textR;
    public bool miniGameisOn;

    private int selectedL;
    private int selectedR;

    private bool leftside;

    private float t;

    private List<Word> wordsInGame = new List<Word>();
    private List<NonObjectWord> wordsInGameNonObject = new List<NonObjectWord>();

    private IEnumerator timerRoutine;

    bool keyAceepted;

    // Start is called before the first frame update
    void Start()
    {
        miniGameisOn = false;
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        textInfoL.GetComponent<TMPro.TextMeshPro>().text = "________________";
        textInfoR.GetComponent<TMPro.TextMeshPro>().text = "________________";

        textInfoL.transform.Find("Highlight").gameObject.SetActive(false);
        textInfoR.transform.Find("Highlight").gameObject.SetActive(false);

        for (int c = 0; c < textL.Length; c++)
        {
            textL[c].SetActive(false);
        }
        for (int c = 0; c < textR.Length; c++)
        {
            textR[c].SetActive(false);
        }
        timerRoutine = coroutineTime();
    }


    // Update is called once per frame
    void Update()
    {
        if (miniGameisOn)
        {
            controller.GetComponent<Controller>().timerText.transform.parent.parent.gameObject.SetActive(true);
            keyAceepted = true;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("lesgo");
                checkingPhase();
            }
        }
    }
    private void OnGUI()
    {
        if (!miniGameisOn)
        {
            return;
        }
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyDown)
        {
            if (!keyAceepted)
            {
                return;
            }
            if(e.keyCode == KeyCode.W)
            {
                if (leftside)
                {
                    leftSelectionchange(-1);
                }
                else
                {
                    rightSelectionchange(-1);
                }
            }
            else if (e.keyCode == KeyCode.S)
            {

                if (leftside)
                {
                    leftSelectionchange(+1);
                }
                else
                {
                    rightSelectionchange(+1);
                }

            }
            else if(e.keyCode == KeyCode.D)
            {
                leftside = !leftside;
                if (leftside)
                {
                    textInfoL.transform.Find("Highlight").gameObject.SetActive(true);
                    textInfoR.transform.Find("Highlight").gameObject.SetActive(false);
                }
                else
                {
                    textInfoL.transform.Find("Highlight").gameObject.SetActive(false);
                    textInfoR.transform.Find("Highlight").gameObject.SetActive(true);
                }
            }
            else if(e.keyCode == KeyCode.A)
            {
                leftside = !leftside;
                if (leftside)
                {
                    textInfoL.transform.Find("Highlight").gameObject.SetActive(true);
                    textInfoR.transform.Find("Highlight").gameObject.SetActive(false);
                }
                else
                {
                    textInfoL.transform.Find("Highlight").gameObject.SetActive(false);
                    textInfoR.transform.Find("Highlight").gameObject.SetActive(true);
                }
            }

        }
    }
    private void leftSelectionchange(int process)
    {
        selectedL += process;
        if (selectedL < 0)
        {
            selectedL = textL.Length - 1;
        }
        else if (selectedL >= textL.Length)
        {
            selectedL = 0;
        }
        while (! textL[selectedL].activeInHierarchy)
        {
            selectedL += process;
            if (selectedL == textL.Length)
            {
                selectedL = 0;
            }
            else if(selectedL < 0)
            {
                selectedL = textL.Length - 1;
            }
        }
        for(int ind = 0; ind < textL.Length;)
        {
            if (textL[ind].activeSelf)
            {
                if (ind == selectedL)
                {
                    textL[ind].transform.Find("Highlight").gameObject.SetActive(true);
                    ind += 1;
                    continue;
                }
                textL[ind].transform.Find("Highlight").gameObject.SetActive(false);
                ind += 1;
            }
            else
            {
                ind += 1;
                continue;
            }
        }
    }
    private void rightSelectionchange(int process)
    {
        selectedR += process;
        if (selectedR < 0)
        {
            selectedR = textR.Length - 1;
        }
        else if (selectedR >= textR.Length)
        {
            selectedR = 0;
        }
        while (!textR[selectedR].activeInHierarchy)
        {
            selectedR += process;
            if (selectedR == textR.Length)
            {
                selectedR = 0;
            }
            else if (selectedR < 0)
            {
                selectedR = textR.Length - 1;
            }
        }

        for (int ind = 0; ind < textR.Length;)
        {
            if (textR[ind].activeSelf)
            {
                if (ind == selectedR)
                {
                    textR[ind].transform.Find("Highlight").gameObject.SetActive(true);
                    ind += 1;
                    continue;
                }
                textR[ind].transform.Find("Highlight").gameObject.SetActive(false);
                ind += 1;
            }
            else
            {
                ind += 1;
                continue;
            }
        }
    }
    public void miniGameStart(string[]left, string[]right, List<Word>word)
    {
        wordsInGame = word;
        textInfoL.GetComponent<TMPro.TextMeshPro>().text = "eþleþtir";
        textInfoR.GetComponent<TMPro.TextMeshPro>().text = "eþleþtir";
        for(int kounter = 0; kounter < left.Length; kounter++)
        {
            textL[kounter].SetActive(true);
            textR[kounter].SetActive(true);
            textL[kounter].GetComponent<TMPro.TextMeshPro>().text = left[kounter];//word_str_en
            textR[kounter].GetComponent<TMPro.TextMeshPro>().text = right[kounter];//word_str_tr
        }

        textInfoL.transform.Find("Highlight").gameObject.SetActive(true);
        textInfoR.transform.Find("Highlight").gameObject.SetActive(false);
        selectedL = 0;
        selectedR = 0;
        leftside = true;
        leftSelectionchange(0);
        rightSelectionchange(0);
        t = 60;
        StartCoroutine(timerRoutine);
        miniGameisOn = true;
    }
    public void miniGameStartNonObject(string[] left, string[] right, List<NonObjectWord> word)
    {
        wordsInGameNonObject = word;
        textInfoL.GetComponent<TMPro.TextMeshPro>().text = "eþleþtir";
        textInfoR.GetComponent<TMPro.TextMeshPro>().text = "eþleþtir";
        for (int kounter = 0; kounter < left.Length; kounter++)
        {
            textL[kounter].SetActive(true);
            textR[kounter].SetActive(true);
            textL[kounter].GetComponent<TMPro.TextMeshPro>().text = left[kounter];//word_str_en
            textR[kounter].GetComponent<TMPro.TextMeshPro>().text = right[kounter];//word_str_tr
        }
        textInfoL.transform.Find("Highlight").gameObject.SetActive(true);
        textInfoR.transform.Find("Highlight").gameObject.SetActive(false);
        selectedL = 0;
        selectedR = 0;
        leftside = true;
        leftSelectionchange(0);
        rightSelectionchange(0);
        t = 60;
        StartCoroutine(timerRoutine);
        miniGameisOn = true;
    }
    private void checkingPhase()
    {
        int selectedidL;
        int selectedidR;
        selectedidL = -1;
        selectedidR = -1;
        if (gameMech.isObjectMech)
        {
            for (int c = 0; c < wordsInGame.Count; c++)
            {
                Debug.Log(selectedL);
                if (textL[selectedL].GetComponent<TMPro.TextMeshPro>().text == wordsInGame[c].wordSTR_EN)
                {
                    selectedidL = wordsInGame[c].id;
                }
                if(textR[selectedR].GetComponent<TMPro.TextMeshPro>().text== wordsInGame[c].wordSTR_TR)
                {
                    selectedidR = wordsInGame[c].id;
                }
            }
            if (selectedidL == -1 || selectedidR == -1)
            {
                Debug.Log("Bug has been occured");
            }
            if (selectedidL == selectedidR)
            {
                textL[selectedL].SetActive(false);
                textR[selectedR].SetActive(false);
                gameMech.GetComponent<AudioSource>().Stop();
                gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().succes;
                gameMech.GetComponent<AudioSource>().Play();
                bool end = true;
                for(int i = 0; i < textL.Length; i++)
                {
                    if (textL[i].activeInHierarchy)
                    {
                        Debug.Log("Hata");
                        end = false;
                    }
                }
                if (end)
                {
                    if(gameMech.dif == GameMech.difficulty.low)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(90);
                    }
                    else if (gameMech.dif == GameMech.difficulty.med)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(120);
                    }
                    else if (gameMech.dif == GameMech.difficulty.hard)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(150);
                    }

                    miniGameisOn = false;
                    textInfoL.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    textInfoR.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    StopCoroutine(timerRoutine);
                    gameMech.stopgameMech("Hepsini Bildin", false);
                }
                else
                {
                    leftSelectionchange(+1);
                    rightSelectionchange(+1);
                }
            }
            else
            {
                gameMech.GetComponent<AudioSource>().Stop();
                gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().fail;
                gameMech.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            for (int c = 0; c < wordsInGameNonObject.Count; c++)
            {
                if (textL[selectedL].GetComponent<TMPro.TextMeshPro>().text == wordsInGameNonObject[c].nameEN)
                {
                    selectedidL = wordsInGameNonObject[c].id;
                }
                if (textR[selectedR].GetComponent<TMPro.TextMeshPro>().text == wordsInGameNonObject[c].nameTR)
                {
                    selectedidR = wordsInGameNonObject[c].id;
                }
            }
            if (selectedidL == -1 || selectedidR == -1)
            {
                Debug.Log("Bug has been occured");
            }
            if (selectedidL == selectedidR)
            {
                textL[selectedL].SetActive(false);
                textR[selectedR].SetActive(false);
                gameMech.GetComponent<AudioSource>().Stop();
                gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().succes;
                gameMech.GetComponent<AudioSource>().Play();
                bool end = true;
                for (int i = 0; i < textL.Length; i++)
                {
                    if (textL[i].activeSelf)
                    {
                        end = false;
                    }
                }
                if (end)
                {
                    if (gameMech.dif == GameMech.difficulty.low)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(200);
                    }
                    else if (gameMech.dif == GameMech.difficulty.med)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(250);
                    }
                    else if (gameMech.dif == GameMech.difficulty.hard)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(300);
                    }
                    miniGameisOn = false;
                    textInfoL.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    textInfoR.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    StopCoroutine(timerRoutine);
                    gameMech.stopgameMech("Hepsini Bildin", false);
                }
                else
                {
                    leftSelectionchange(+1);
                    rightSelectionchange(+1);
                }
            }
            else
            {
                gameMech.GetComponent<AudioSource>().Stop();
                gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().fail;
                gameMech.GetComponent<AudioSource>().Play();
            }
        }
    }
    public void playerInterrupt()
    {
        miniGameisOn = false;
        textInfoL.GetComponent<TMPro.TextMeshPro>().text = "________________";
        textInfoR.GetComponent<TMPro.TextMeshPro>().text = "________________";
        for (int kounter = 0; kounter < textL.Length; kounter++)
        {
            textL[kounter].SetActive(false);
            textR[kounter].SetActive(false);
        }
        StopCoroutine(timerRoutine);
    }
    IEnumerator coroutineTime()
    {
        while (t >= 0)
        {
            controller.GetComponent<Controller>().timerText.text = t.ToString();
            t -= 1;
            yield return new WaitForSeconds(1f);
        }
        miniGameisOn = false;
        textInfoL.GetComponent<TMPro.TextMeshPro>().text = "________________";
        textInfoR.GetComponent<TMPro.TextMeshPro>().text = "________________";
        for (int kounter = 0; kounter < textL.Length; kounter++)
        {
            textL[kounter].SetActive(false);
            textR[kounter].SetActive(false);
        }
        gameMech.stopgameMech("SüreDoldu",false);

    }

}
