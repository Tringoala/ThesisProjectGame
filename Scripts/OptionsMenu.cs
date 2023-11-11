using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    public Toggle fullscreenTog;

    public GameObject MainMenuCanvas;
    public GameObject OptionsCanvas;


    private GameObject mc;

    // Start is called before the first frame update
    void Start()
    {
        mc = GameObject.Find("Main Camera");

        fullscreenTog.isOn = Screen.fullScreen;

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        mc.GetComponent<AudioSource>().Play();
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void ChangeFullScreen()
    {
        mc.GetComponent<AudioSource>().Play();
        Screen.fullScreen = !Screen.fullScreen;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseOptions()
    {
        mc.GetComponent<AudioSource>().Play();
        OptionsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

}
