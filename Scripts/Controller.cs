using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Controller : MonoBehaviour
{
    [HideInInspector]
    public KeyCode pauseButton;
    [HideInInspector]
    public KeyCode codexButton;
    [HideInInspector]
    public KeyCode interactButton;

    public GameObject codexPanel;
    public GameObject MenuPanel;
    public GameObject MainPanel;
    public GameObject popUpTextPrefab;
    public GameObject interactionGO;
    public GameObject pointDisplay;
    public GameObject DialogPanel;
    public GameObject SceneStarterPanel;

    public Text timerText;

    public bool interactable;
    private GameObject currentInteract;

    public GameObject infoCodex;
    public GameObject infoPause;

    private bool stopUI;

    public NonObjectWord blankitem;
    public DataController dc;

    public GameObject mc;

    public bool inGameMech;
    public bool inDialog;


    //saveSystem
    public int[] containedID;
    public int[] learnedID;
    private List<int> learnedIDList = new List<int>();
    public int[] learnedContainersID;
    private List<int> learnedContainersList = new List<int>();

    public GameObject ünlemPrefab;

    public bool newgame;
    public bool scenestartbool;
    // Start is called before the first frame update
    private void Awake()
    {
        ObjectController[] obj = GameObject.FindObjectsOfType<ObjectController>();
        ContainerManager[] cmobj = GameObject.FindObjectsOfType<ContainerManager>();
        for(int c = 0; c < obj.Length; c++)
        {
            obj[c].ünlemOBJ = ünlemPrefab;
        }
        for (int c = 0; c < cmobj.Length; c++)
        {
            cmobj[c].ünlemOBJ = ünlemPrefab;
        }
        newgame = PlayerPrefs.GetInt("newGame") == 1 ? true : false;
    }
    void Start()
    {
        Cursor.visible = false;
        inGameMech = false;
        inDialog = false;
        gameObject.GetComponent<DataController>().LoaditemData();
        scenestartbool = true;
        StartCoroutine(sceneStart());
        if (newgame)
        {
            Invoke("fillContainer", 2f);
            mc.GetComponent<playerOOP>().addPoint(-mc.GetComponent<playerOOP>().getPoint());

        }
        else
        {
            Invoke("loadGame", 2f);
        }
        pauseButton = KeyCode.P;
        //pause button will be inhereted from main menu settings key bindings or it will be always 'p'
        codexButton = KeyCode.Q;
        //codex button will be inhereted from main menu settings key bindings or it will be always 'Q'
        interactButton = KeyCode.E;
        //interact button will be inhereted from main menu settings key bindings or it will be always 'E'

        mc = GameObject.FindGameObjectWithTag("MC");

        //pause initialize
        infoPause.GetComponentInChildren<Text>().text = "Oyunu durdurmak için '" + pauseButton.ToString() + "'e basýn";
        //Codex initialize
        infoCodex.GetComponentInChildren<Text>().text = "Codexi açmak için '" + codexButton.ToString() + "'e basýn";

        timerText.transform.parent.parent.gameObject.SetActive(false);

        codexPanel.SetActive(false);
        MenuPanel.SetActive(false);
        DialogPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        pointDisplay.GetComponentInChildren<Text>().text = mc.GetComponent<playerOOP>().getPoint().ToString();
        if (inGameMech)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                closeGameMech();
            }
            return;
        }
        else if (inDialog)
        {
            return;
        }
        else if (scenestartbool)
        {
            return;
        }

        if (Input.GetKeyDown(pauseButton))
        {
            pauseGame();
        }
        else if (Input.GetKeyDown(codexButton))
        {
            codexOpen();
        }
        else if (Input.GetKeyDown(interactButton))
        {
            if (!stopUI)
            {
                if (interactable)
                {
                    interactObject();
                }
            }
        }
    }
    private void codexOpen()
    {
        if (codexPanel.activeSelf)
        {
            Cursor.visible = false;
            codexPanel.SetActive(false);
            codexPanel.GetComponentInChildren<CodexController>().codexClosed();
            stopUI = false;
        }
        else
        {
            if (!stopUI)
            {
                Cursor.visible = true;
                codexPanel.SetActive(true);
                codexPanel.GetComponentInChildren<CodexController>().codexOpenConnector();
                stopUI = true;
            }
        }
    }
    private void learnObject()
    {
        if (currentInteract != null)
        {
            mc.GetComponent<AudioSource>().Play();
            interactionGO.SetActive(false);
            interactionGO.SetActive(true);
            currentInteract.gameObject.tag = "LearnedObject";
            currentInteract.GetComponent<ObjectController>().word.isLearned = true;

            //Coroutinestart popup
            GameObject temptext = GameObject.Instantiate(popUpTextPrefab.gameObject, MainPanel.transform);
            temptext.GetComponent<Text>().text = currentInteract.GetComponent<ObjectController>().word.wordSTR_EN + "'i öðrendin";
            StartCoroutine(popUpAnim(0.05f,temptext.GetComponent<Text>()));
        }
        else
        {
            return;
        }
    }
    public IEnumerator popUpAnim(float routineDur,Text popupText)
    {

        while (popupText.color.a > 0)
        {
            Color tempTextColor = new Color(popupText.color.r, popupText.color.g, popupText.color.b, popupText.color.a-routineDur);
            popupText.color = tempTextColor;
            popupText.gameObject.transform.position = new Vector3(popupText.gameObject.transform.position.x, popupText.gameObject.transform.position.y + routineDur * 50, popupText.gameObject.transform.position.z);
            if (popupText.color.a <= 0)
            {
                Destroy(popupText.gameObject);
            }
            yield return new WaitForSeconds(routineDur);
        }
    }

    private void fillContainer()
    {
        GameObject[] containers = GameObject.FindGameObjectsWithTag("Container");
        List<noWItemData> dataList = dc.itemDatabase;
        for (int k=0;k < containers.Length; k++)
        {
            for(int i = 0; i < containers[k].GetComponent<ContainerManager>().containsWord;)
            {
                int tempID = UnityEngine.Random.Range(0, dataList.Count);
                NonObjectWord tempObject = new NonObjectWord(blankitem);


                tempObject.id = dataList[tempID].id;
                tempObject.nameTR = dataList[tempID].wordTR;
                tempObject.nameEN = dataList[tempID].wordEn;
                tempObject.desc = dataList[tempID].desc;
                tempObject.isLearned = false;
                tempObject.isStudied = false;
                tempObject.contained = true;
                dataList.RemoveAt(tempID);
                containers[k].GetComponent<ContainerManager>().nonObjectWordsdb.Add(tempObject);
                i += 1;
            }
        }
    }
    private void startDialog()
    {
        inDialog = true;
        MainPanel.SetActive(false);
        currentInteract.GetComponent<DialogueManager>().startDialog();
    }

    private void interactObject()
    {
        if (currentInteract.transform.CompareTag("LearnableObject"))
        {   
            learnObject();
        }
        else if (currentInteract.transform.CompareTag("GameMech"))
        {
            openGameMech();
        }
        else if (currentInteract.transform.CompareTag("UnaccesibleRect"))
        {
            teleportMC();
        }
        else if (currentInteract.transform.CompareTag("Container"))
        {
            containerLearnWords();
        }
        else if (currentInteract.transform.CompareTag("NPC"))
        {
            startDialog();
        }
    }
    private void teleportMC()
    {
        if (currentInteract.GetComponent<ScenePasser>().verticalPasser)
        {
            if (currentInteract.transform.position.z > mc.transform.position.z)
            {
                mc.transform.position = currentInteract.GetComponent<ScenePasser>().positivePos.position;
            }
            else
            {
                mc.transform.position = currentInteract.GetComponent<ScenePasser>().negativePos.position;
            }

        }
        else
        {
            if (currentInteract.transform.position.y > mc.transform.position.y)
            {
                mc.transform.position = currentInteract.GetComponent<ScenePasser>().positivePos.position;
            }
            else
            {
                mc.transform.position = currentInteract.GetComponent<ScenePasser>().negativePos.position;
            }
        }
    }
    private void containerLearnWords()
    {
        mc.GetComponent<AudioSource>().Play();
        interactionGO.SetActive(false);
        interactionGO.SetActive(true);
        GameObject container = currentInteract;
        for(int c = 0; c < container.GetComponent<ContainerManager>().containsWord; c++)
        {
            container.GetComponent<ContainerManager>().nonObjectWordsdb[c].isLearned = true;
            learnedIDList.Add(container.GetComponent<ContainerManager>().nonObjectWordsdb[c].id);
        }
        container.GetComponent<ContainerManager>().isLearned = true;
        learnedContainersList.Add(container.GetComponent<ContainerManager>().containerID);
        GameObject temptext = GameObject.Instantiate(popUpTextPrefab.gameObject, MainPanel.transform);
        temptext.GetComponent<Text>().text ="Kelimeleri öðrendin\nKontrol etmek için kodekse bakýn";
        StartCoroutine(popUpAnim(0.05f, temptext.GetComponent<Text>()));
    }
    private void openGameMech()
    {
        inGameMech = true;
        currentInteract.GetComponent<GameMech>().gameMechStart(mc.GetComponentInChildren<Camera>().gameObject);
    }
    private void closeGameMech()
    {
        currentInteract.GetComponent<GameMech>().stopgameMech("Kullanýcý tarafaýndan durduruldu",false);
    }


    public void interactChange(GameObject aka)
    {
        currentInteract = aka;
    }

    //GameInMenu
    private void pauseGame()
    {
        if (! MenuPanel.activeSelf)
        {
            Cursor.visible = true;
            stopUI = true;
            Time.timeScale = 0f;
            MenuPanel.SetActive(true);
        }
    }
    public void resumeGame()
    {
        Cursor.visible = false;
        stopUI = false;
        Time.timeScale = 1f;
        MenuPanel.SetActive(false);
    }
    public void toMainMenu()
    {
        stopUI = false;
        Time.timeScale = 1f;
        MenuPanel.SetActive(false);
        //savePanelOpen
        saveGame();
        StartCoroutine(sceneStart());
        Invoke("backToMainScene", 2f);
    }
    private void backToMainScene()
    {
        SceneManager.LoadScene(0);
    }
    private void loadGame()
    {
        DataElem data = SaveSystem.LoadPlayer();
        
        mc.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        mc.GetComponent<playerOOP>().addPoint(data.playerPoint);

        learnedContainersID = data.learnedContainerID;
        learnedID = data.learnedID;
        containedID = data.containedID;
        fillOnLoadContainer();

    }
    private void fillOnLoadContainer()
    {
        GameObject[] containers = GameObject.FindGameObjectsWithTag("Container");
        for(int k = 0; k < containers.Length; k++)
        {
            bool notLearned = true;
            for(int j = 0; j < learnedContainersID.Length; j++)
            {
                if (containers[k].GetComponent<ContainerManager>().containerID == learnedContainersID[j])
                {
                    containers[k].GetComponent<ContainerManager>().isLearned = true;
                    notLearned = false;
                    break; //unique
                }
            }
            if (notLearned)
            {
                containers[k].GetComponent<ContainerManager>().isLearned = false;
            }
        }
        int learnedcounter = 0;
        int containedcounter = 0;
        for (int i = 0; i < containers.Length; i++)
        {
            if (containers[i].GetComponent<ContainerManager>().isLearned)
            {
                for(int c = 0; c < containers[i].GetComponent<ContainerManager>().containsWord; c++)
                {
                    NonObjectWord tempElem = new NonObjectWord(blankitem);
                    noWItemData item = gameObject.GetComponent<DataController>().itemDatabase.Find(x => x.id == learnedID[learnedcounter]);
                    tempElem.id = item.id;
                    tempElem.nameEN = item.wordEn;
                    tempElem.nameTR = item.wordTR;
                    tempElem.desc = item.desc;
                    tempElem.isLearned = true;
                    tempElem.isStudied = false;
                    tempElem.contained = true;
                    learnedcounter += 1;
                    containers[i].GetComponent<ContainerManager>().nonObjectWordsdb.Add(tempElem);
                }
            }
            else
            {
                for(int c = 0; c < containers[i].GetComponent<ContainerManager>().containsWord; c++)
                {
                    NonObjectWord tempElem = new NonObjectWord(blankitem);
                    noWItemData item = gameObject.GetComponent<DataController>().itemDatabase.Find(x => x.id == containedID[containedcounter]);
                    tempElem.id = item.id;
                    tempElem.nameEN = item.wordEn;
                    tempElem.nameTR = item.wordTR;
                    tempElem.desc = item.desc;
                    tempElem.isLearned = false;
                    tempElem.isStudied = false;
                    tempElem.contained = true;
                    containedcounter += 1;
                    containers[i].GetComponent<ContainerManager>().nonObjectWordsdb.Add(tempElem);
                }
            }
        }
    }
    private void saveGame()
    {
        GameObject[] containers = GameObject.FindGameObjectsWithTag("Container");
        List<int> tempcontainerID = new List<int>();
        for(int c = 0; c < containers.Length; c++)
        {
            if (!containers[c].GetComponent<ContainerManager>().isLearned)
            {
                for (int x = 0; x < containers[c].GetComponent<ContainerManager>().nonObjectWordsdb.Count; x++)
                {
                    tempcontainerID.Add(containers[c].GetComponent<ContainerManager>().nonObjectWordsdb[x].id);
                }
            }
        }
        containedID = new int[tempcontainerID.Count];
        for(int k = 0; k < containedID.Length; k++)
        {
            containedID[k] = tempcontainerID[k];
        }
        learnedID = new int[learnedIDList.Count];
        for (int k = 0; k < learnedID.Length; k++)
        {
            learnedID[k] = learnedIDList[k];
        }
        learnedContainersID = new int[learnedContainersList.Count];
        for (int k = 0; k < learnedContainersID.Length; k++)
        {
            learnedContainersID[k] = learnedContainersList[k];
        }


        SaveSystem.SavePlayer(mc.GetComponent<playerOOP>(), gameObject.GetComponent<Controller>());
    }

    IEnumerator sceneStart()
    {
        float i = 2;
        scenestartbool = true;
        yield return new WaitForSeconds(2f);
        while (i > 0)
        {
            if (SceneStarterPanel.activeInHierarchy)
            {
                i -= 0.02f;
                SceneStarterPanel.GetComponent<Image>().color = new Color(0, 0, 0, SceneStarterPanel.GetComponent<Image>().color.a - 0.01f);
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                i -= 0.02f;
                SceneStarterPanel.GetComponent<Image>().color = new Color(0, 0, 0, SceneStarterPanel.GetComponent<Image>().color.a + 0.01f);
                yield return new WaitForSeconds(0.02f);
            }
        }
        scenestartbool = false;
        SceneStarterPanel.SetActive(false);
    }
}