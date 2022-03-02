using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    public Image sideMenu;
    public GameObject btnOpen;
    public GameObject btnClose;
    //
   
   
    void Start()
    {
        sideMenu.GetComponent<Image>().transform.localPosition = new Vector3(-655f,0f,0f);
    }

    IEnumerator sideMenuOpen()
    {
        btnOpen.SetActive(false);
        btnClose.SetActive(true);
        float index = -655f;
        while (index < -121)
        {
            // new Vector3(-93,0,0)
            sideMenu.GetComponent<Image>().transform.localPosition = new Vector3(index, 0f, 0);
            index = index + 53.5f;
            yield return new WaitForSeconds(0.0000001f);
        }
    }

    IEnumerator sideMenuClose()
    {
        btnOpen.SetActive(true);
        btnClose.SetActive(false);
        float index = -173.5f;
        while (index >= -655)
        {
            // new Vector3(-93,0,0)
            sideMenu.GetComponent<Image>().transform.localPosition = new Vector3(index, 0, 0);
            index = index - 48.15f;
            yield return new WaitForSeconds(0.000000000001f);
        }
    
    }

    public void opsenSideMenu()
    {
        StartCoroutine(sideMenuOpen());
    }
    public void closeSideMenu()
    {
        StartCoroutine(sideMenuClose());       
    }
    public void goToFlashCardTraining()
    {        
        SceneManager.LoadScene("flashcard");
    }

    public void goToAllWords()
    {
        SceneManager.LoadScene("Searching");
    }

    public void quitFromApp()
    {
        Application.Quit();
    }
}
