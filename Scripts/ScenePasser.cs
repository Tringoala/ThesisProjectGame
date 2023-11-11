using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePasser : MonoBehaviour
{
    public Transform positivePos;
    public Transform negativePos;
    public GameObject interactionPanel;
    private Controller gameController;
    public bool needThreshold;
    public int thresholdPoint;
    public bool verticalPasser;
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        interactionPanel = GameObject.FindGameObjectWithTag("MC").GetComponentInChildren<InteractionController>().interactionPanel;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MC"))
        {
            interactionPanel.SetActive(true);
            if (needThreshold)
            {
                if (other.GetComponent<playerOOP>().getPoint() < thresholdPoint)
                {
                    interactionPanel.GetComponentInChildren<Text>().text = "Puanýn yeterli deðil";
                    return;
                }
            }
            if (verticalPasser)
            {
                if (gameObject.transform.position.z > other.transform.position.z)
                {
                    interactionPanel.GetComponentInChildren<Text>().text = "Karþýya geçmek için" + gameController.interactButton.ToString() + "'a basýn";
                }
                else
                {
                    interactionPanel.GetComponentInChildren<Text>().text = "Karþýya geçmek için" + gameController.interactButton.ToString() + "'a basýn";
                }
            }
            else
            {
                if (gameObject.transform.position.z > other.transform.position.z)
                {
                    interactionPanel.GetComponentInChildren<Text>().text = "Karþýya geçmek için" + gameController.interactButton.ToString() + "'a basýn";
                }
                else
                {
                    interactionPanel.GetComponentInChildren<Text>().text = "Karþýya geçmek için" + gameController.interactButton.ToString() + "'a basýn";
                }
            }
            gameController.interactable = true;
            gameController.interactChange(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MC"))
        {
            interactionPanel.SetActive(false);
            gameController.interactable = false;
            gameController.interactChange(null);
        }
    }
}
