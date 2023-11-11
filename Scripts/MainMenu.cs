using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject OptionsCanvas;

    public GameObject continueBut;
    public GameObject saveDeleteBut;
    public GameObject darkScreen;
    
    private GameObject mc;
    private string path;

    private void Awake()
    {
        path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            continueBut.SetActive(true);
            saveDeleteBut.SetActive(true);
        }
        else
        {
            continueBut.SetActive(false);
            saveDeleteBut.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mc = GameObject.Find("Main Camera");
        darkScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueGame()
    {
        mc.GetComponent<AudioSource>().Play();
        bool val = false;
        PlayerPrefs.SetInt("newGame", val ? 1 : 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
    public void deleteSave()
    {
        mc.GetComponent<AudioSource>().Play();
        continueBut.SetActive(false);
        saveDeleteBut.SetActive(false);
        File.Delete(path);
    }
    public void StartGame()
    {
        mc.GetComponent<AudioSource>().Play();
        bool val = true;
        PlayerPrefs.SetInt("newGame", val ? 1 : 0);
        PlayerPrefs.Save();
        StartCoroutine(sceneStartAnim());
        Invoke("sceneStartFunc", 1f);
    }
    private void sceneStartFunc()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenOptions()
    {
        mc.GetComponent<AudioSource>().Play();
        MainMenuCanvas.SetActive(false);
        OptionsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator sceneStartAnim()
    {
        float i = 1;
        float cont = 0.2f;
        darkScreen.SetActive(true);
        while (i > 0)
        {
            if (darkScreen.GetComponent<Image>().color.a >= 1)
            {
                darkScreen.GetComponentInChildren<Text>().text = "Uyu";
                darkScreen.GetComponentInChildren<Text>().color = new Color(255, 255, 255, darkScreen.GetComponentInChildren<Text>().color.a - 0.25f);
                cont = 0.05f;
            }
            else
            {
                darkScreen.GetComponent<Image>().color = new Color(0, 0, 0, darkScreen.GetComponent<Image>().color.a + 0.25f);
            }
            i -= cont;
            yield return new WaitForSeconds(cont);
        }
    }
}
