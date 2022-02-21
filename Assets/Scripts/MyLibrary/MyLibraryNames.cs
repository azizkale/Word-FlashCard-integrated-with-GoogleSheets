using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyLibraryNames : MonoBehaviour
{
    GameObject cloneFileCard;
    public GameObject subMenuPrefab;
    public GameObject prefabFileCard; //prefab
    public GameObject prefabSubMenu_RenameFunction;
    public GameObject prefabAlertWarning;
    public GameObject prefabDeleteCard;

    GameObject cloneSubMenu;
    public GameObject canvas;
    public GameObject scrolContainer;
    List<AllLibrariesInfo> allLibrariesInfo = new List<AllLibrariesInfo>();
    void Start()
    {
        //initializing control
        if (Read.getListAllLibrariesInfo() == null)
        {
            initializing.initializingtheapp();
            SceneManager.LoadScene("MyLibraryNames");
        }
        else
        {
            allLibrariesInfo = Read.getListAllLibrariesInfo();
            createFileNamesCards();

        }
    }

    public void createFileNamesCards()
    {
        try
        {
            foreach (AllLibrariesInfo labinfo in allLibrariesInfo)
            {
                cloneFileCard = Instantiate(prefabFileCard, scrolContainer.transform.position, Quaternion.identity, scrolContainer.transform);
                //
                TextMeshProUGUI fileName = cloneFileCard.transform.Find("Button").transform.Find("TMP_FileName").GetComponent<TextMeshProUGUI>();
                fileName.text = CommonVariables.charachterLimit(labinfo.name, 14);
                //
                TextMeshProUGUI wordsCount = cloneFileCard.transform.Find("Button").transform.Find("TMP_TotalWordsCount").GetComponent<TextMeshProUGUI>();
                wordsCount.text = labinfo.wordsCount.ToString();
                //
                TextMeshProUGUI languageInfo = cloneFileCard.transform.Find("Button").transform.Find("TMP_LanguageInfo").GetComponent<TextMeshProUGUI>();
                languageInfo.text = CommonVariables.charachterLimit(labinfo.language.ToString(), 17);

                //button below opens library contetnt
                cloneFileCard.transform.Find("Button").GetComponent<Button>().
                    onClick.AddListener(() => {
                        CommonVariables.libraryName = labinfo.name;
                        SceneManager.LoadScene("LibraryContent");
                    });
                //submenu-button
                cloneFileCard.transform.Find("Button").transform.Find("btn_SubMenu").GetComponent<Button>().onClick.AddListener(() => {
                    createSubMenuRuntime(subMenuPrefab, labinfo);
                });
            }
        }
        catch (System.Exception e)
        {          
            
            initializing.initializingtheapp();
         
        }
      
    } 

    void createSubMenuRuntime(GameObject submenuprefab, AllLibrariesInfo lab)
    {
        DestroyImmediate(cloneSubMenu);

        cloneSubMenu = Instantiate(
               submenuprefab,
               canvas.transform.position,
               Quaternion.identity,
               canvas.transform);
        cloneSubMenu.transform.localPosition = Vector3.zero;

        //rename-button
        cloneSubMenu.transform.Find("btn_Rename").GetComponent<Button>().onClick.AddListener(() =>
        {
            SubMenu_OnMyLibrariesNames.rename(prefabSubMenu_RenameFunction, cloneSubMenu, prefabAlertWarning, lab,canvas);
        });

        //delete-button
        cloneSubMenu.transform.Find("btn_Delete").GetComponent<Button>().onClick.AddListener(() =>{
            SubMenu_OnMyLibrariesNames.delete(lab,canvas,prefabDeleteCard);
        });

        //cancel-button
        cloneSubMenu.transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(() => {
            DestroyImmediate(cloneSubMenu);
        });

    }
}
