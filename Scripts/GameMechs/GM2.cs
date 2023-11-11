using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class GM2 : MonoBehaviour
{
    private Controller controller;

    public GameMech gameMech;
    public GameObject textInfo;
    public GameObject textEng;
    public GameObject sprite;
    public GameObject textTur;
    public bool miniGameisOn;

    private float t;

    private List<Word> wordsInGame = new List<Word>();
    private List<NonObjectWord> wordsInGameNonObject = new List<NonObjectWord>();


    private int counter;
    private string sol;

    private IEnumerator timerRoutine;
    // Start is called before the first frame update
    void Start()
    {
        miniGameisOn = false;
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        textInfo.GetComponent<TMPro.TextMeshPro>().text = "________________";
        sprite.SetActive(false);
        textEng.GetComponent<TMPro.TextMeshPro>().text = "________________";
        textTur.GetComponent<TMPro.TextMeshPro>().text = "________________";
        timerRoutine = coroutinetimer();
    }


    // Update is called once per frame
    void Update()
    {
        if (miniGameisOn)
        {
            controller.GetComponent<Controller>().timerText.transform.parent.parent.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                checkingPhase();
            }
        }
    }
    public void addStr(KeyCode kc)
    {
        if (kc == KeyCode.Backslash)//ç
        {
            sol += "ç";
        }
        else if (kc == KeyCode.RightBracket)//ü
        {
            sol += "ü";
        }
        else if (kc == KeyCode.LeftBracket)//ð
        {
            sol += "ð";
        }
        else if (kc == KeyCode.Quote)//i
        {
            sol += "i";
        }
        else
        {
            sol += kc.ToString().ToLower();
        }
        textTur.GetComponent<TMPro.TextMeshPro>().text = sol;
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
            bool keyAceepted;
            keyAceepted = false;
            for (int i = 97;i < 122; i++)
            {
                KeyCode value = (KeyCode)i;
                if (value == e.keyCode)
                {
                    keyAceepted = true;
                    break;
                }
            }
            if (e.keyCode == KeyCode.Backslash)//ç
            {
                keyAceepted = true;
            }
            else if (e.keyCode == KeyCode.RightBracket)//ü
            {
                keyAceepted = true;
            }
            else if (e.keyCode == KeyCode.LeftBracket)//ð
            {
                keyAceepted = true;
            }
            else if (e.keyCode == KeyCode.Quote)//i
            {
                keyAceepted = true;
            }

            if (keyAceepted)
            {
                if (miniGameisOn)
                {
                    addStr(e.keyCode);
                }
            }
        }
    }
    public void miniGameStart(List<Word> words)
    {
        counter = 0;
        wordsInGame = words;
        textInfo.GetComponent<TMPro.TextMeshPro>().text = "Kelimeyi Tamamla";
        sprite.SetActive(true);
        sprite.GetComponent<SpriteRenderer>().sprite = words[counter].word_UI_Image;
        textEng.SetActive(false);
        textTur.GetComponent<TMPro.TextMeshPro>().text = "_________________";
        sol = "";
        t = 60;
        miniGameisOn = true;
        StartCoroutine(timerRoutine);
    }
    public void miniGameStartNonObject(List<NonObjectWord> words)
    {
        counter = 0;
        wordsInGameNonObject = words;
        sprite.SetActive(false);
        textEng.SetActive(true);
        textEng.GetComponent<TMPro.TextMeshPro>().text = words[counter].nameTR;
        textTur.GetComponent<TMPro.TextMeshPro>().text = "_________________";
        sol = "";
        t = 60;
        miniGameisOn = true;
        StartCoroutine(timerRoutine);
    }
    private void checkingPhase()
    {
        if (gameMech.isObjectMech)
        {
            if (sol == wordsInGame[counter].wordSTR_EN.ToLower())
            {
                gameMech.GetComponent<AudioSource>().Stop();
                gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().succes;
                gameMech.GetComponent<AudioSource>().Play();
                counter += 1;
                if(counter >= wordsInGame.Count)
                {
                    if (gameMech.dif == GameMech.difficulty.low)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(30);
                    }
                    else if (gameMech.dif == GameMech.difficulty.med)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(60);
                    }
                    else if (gameMech.dif == GameMech.difficulty.hard)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(90);
                    }
                    miniGameisOn = false;
                    textInfo.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    sprite.SetActive(false);
                    textEng.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    textTur.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    StopCoroutine(timerRoutine);
                    gameMech.stopgameMech("Hepsini bildin", false);
                }
                else
                {
                    gameMech.GetComponent<AudioSource>().Stop();
                    gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().fail;
                    gameMech.GetComponent<AudioSource>().Play();
                    sprite.GetComponent<SpriteRenderer>().sprite = wordsInGame[counter].word_UI_Image;
                }
            }
                sol = "";
                textTur.GetComponent<TMPro.TextMeshPro>().text = sol;
        }
        else
        {
            if (sol == wordsInGameNonObject[counter].nameEN.ToLower())
            {
                gameMech.GetComponent<AudioSource>().Stop();
                gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().succes;
                gameMech.GetComponent<AudioSource>().Play();
                counter += 1;
                if (counter >= wordsInGameNonObject.Count)
                {

                    if (gameMech.dif == GameMech.difficulty.low)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(150);
                    }
                    else if (gameMech.dif == GameMech.difficulty.med)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(200);
                    }
                    else if (gameMech.dif == GameMech.difficulty.hard)
                    {
                        controller.GetComponent<Controller>().mc.GetComponent<playerOOP>().addPoint(250);
                    }

                    miniGameisOn = false;
                    textInfo.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    sprite.SetActive(false);
                    textEng.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    textTur.GetComponent<TMPro.TextMeshPro>().text = "________________";
                    StopCoroutine(timerRoutine);
                    gameMech.stopgameMech("Hepsini bildin", false);
                }
                else
                {
                    gameMech.GetComponent<AudioSource>().Stop();
                    gameMech.GetComponent<AudioSource>().clip = gameMech.GetComponent<GameMech>().fail;
                    gameMech.GetComponent<AudioSource>().Play();
                    textEng.GetComponent<TMPro.TextMeshPro>().text = wordsInGameNonObject[counter].nameTR;
                }
            }
            sol = "";
            textTur.GetComponent<TMPro.TextMeshPro>().text = sol;

        }
    }
    public void playerInterrupt()
    {
        miniGameisOn = false;
        StopCoroutine(timerRoutine);
    }
    IEnumerator coroutinetimer()
    {
        controller.GetComponent<Controller>().timerText.text = t.ToString();
        while (t >= 0)
        {
            yield return new WaitForSeconds(1f);
            t -= 1;
            controller.GetComponent<Controller>().timerText.text = t.ToString();
        }
        miniGameisOn = false;
        textInfo.GetComponent<TMPro.TextMeshPro>().text = "________________";
        sprite.SetActive(false);
        textEng.GetComponent<TMPro.TextMeshPro>().text = "________________";
        textTur.GetComponent<TMPro.TextMeshPro>().text = "________________";
        gameMech.stopgameMech("Süre Doldu",false);
    }
}
