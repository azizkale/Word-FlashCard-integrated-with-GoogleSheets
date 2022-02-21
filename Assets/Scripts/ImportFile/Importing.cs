using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine.SceneManagement;

public class Importing : MonoBehaviour
{
    //Lists which are used to get ready the data to create new files from it==========
    // all lines of the importing file
    private string[] allLines;

    ////all lines are converted to type-Word and added to List<Word> wordsList
    private List<Word> wordsList = new List<Word>();

    //wordList is converted to libraries with their names
    private List<(Library lib, string libname)> libraryList = new List<(Library lib, string libname)>();

    //according to the count of different languages in the importing file, it is created language-cards from prefabLanguageCard
    List<GameObject> listClonePrefabLanguageCard = new List<GameObject>();
    //===================================================================

    //to open select-library-menu========================================
    //to append library-name-cards to prefabselectLibrary
    public GameObject prefabselectLibraryBackground;
    public GameObject prefabselectLibraryButton;
    //===================================================================


    public GameObject canvas;// to append language menu to canvas
    public GameObject prefabIportFileMenu;
    GameObject clonePrefabIportFileMenu;
    public GameObject prefabLanguageCard;
    public GameObject prefabGiveName;
    GameObject clonePrefabGiveName;
    public GameObject prefabAlertWarning;
    void Start()
    {       
        //PlayerPrefs.DeleteAll();       
    }

    IEnumerator getFileFromDeviceAndCreateNewLibraries()
    {
        string[] filters = { ".tsv" };
        FileBrowser.SetFilters(false, filters);
        // gets file path in device
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select a .tsv file", "Load");       

        if (FileBrowser.Success)
        {
            string destinationPath = FileBrowser.Result[0];           

            allLines = File.ReadAllLines(destinationPath);

            wordsList.Clear();
            foreach (string line in allLines)
            {
                string[] str = line.Split('\t');
                Word word = new Word();
                word.theWord = str[2];
                word.meaning = str[3];
                word.languageFrom = str[0];
                word.languageTo = str[1];
                wordsList.Add(word);
            }

            createlibrariesListByUsingImportedFile();
            createSelectLanguageMenu();
        }
    }  

  // create files using loaded file according to different languages it has
  List<(Library lib, string libname)> createlibrariesListByUsingImportedFile()
    {
        libraryList.Clear();
        foreach (Word word in wordsList)
        {         
            string name = word.languageFrom + "-" + word.languageTo;
            if(libraryList.Contains(libraryList.Find(li => li.libname == name))) // if the libarary exist in the list libraries
            {
                Library lib = libraryList.Find(li => li.libname == name).lib;
                lib.name = name;
                lib.words.Add(word);
            }
            else
            {

                Library lib = new Library();
                lib.name = name;
                lib.words = new List<Word>();
                lib.words.Add(word);
                
                libraryList.Add((lib, name));
            }
           
        }
        return libraryList;      
    }

    private void createSelectLanguageMenu()
    {
        clonePrefabIportFileMenu = Instantiate(prefabIportFileMenu, canvas.transform.position, Quaternion.identity, canvas.transform) as GameObject;
        //
        clonePrefabIportFileMenu.transform.Find("Scroll View").transform.position = canvas.transform.position;

        //CreateNewLibrary-button
        clonePrefabIportFileMenu.transform.Find("Footer").transform.Find("btn_CreateNewLibrary").GetComponent<Button>().onClick.AddListener(() => {
            if(libraryList.Count > 0 && ImportingFuntions.wordsToCreateNewLibraryOnTheDevice.Count > 0)
            {
                //givename-card to the creating file
                clonePrefabGiveName = Instantiate(prefabGiveName, canvas.transform.position, Quaternion.identity, canvas.transform) as GameObject;
                //to disappear library-names-scroll
                clonePrefabIportFileMenu.SetActive(false);

                //CREATE button on the prefabGiveName
                clonePrefabGiveName.transform.Find("btn_Create").GetComponent<Button>().onClick.AddListener(() => {

                    string fileName = clonePrefabGiveName.transform.Find("InputField (TMP)").transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>().text;

                    createNewLibrary(fileName, clonePrefabIportFileMenu);
                    //closes the prefab prefabGiveName
                    DestroyImmediate(clonePrefabGiveName);
                    //reopens library-names-scroll (language menu)
                    clonePrefabIportFileMenu.SetActive(true);

                });

                //CANCEL button on the prefabGiveName
                clonePrefabGiveName.transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(() => {
                    clonePrefabIportFileMenu.SetActive(true);
                    DestroyImmediate(clonePrefabGiveName);
                });
            }
            else
            {
                alertWarning.pleaseSelectALibrary(prefabAlertWarning, canvas);
            }        
        });

        //OpenTheSelectLibraryMenu-Button
        clonePrefabIportFileMenu.transform.Find("Footer").transform.Find("btn_OpenTheSelectLibraryMenu").GetComponent<Button>().onClick.AddListener(()=> {
            if (libraryList.Count == 0)
            {
                alertWarning.thereIsNoLibraryToSelect(prefabAlertWarning, canvas);
            }          
            else
            {
                CommonVariables.selectedLibraryContetnt = null;

                //the library on the device is calling and assigned to
                // "CommonVariables.selectedLibraryContetnt"
                SelectLibrarMenu.createSelectMenu(canvas, prefabselectLibraryBackground, prefabselectLibraryButton);
                //
                Button selectMenu = clonePrefabIportFileMenu.transform.Find("Footer").transform.Find("btn_OpenTheSelectLibraryMenu").GetComponent<Button>();
                Button addToLibrary = clonePrefabIportFileMenu.transform.Find("Footer").transform.Find("btn_OK").GetComponent<Button>();
                selectMenu.gameObject.SetActive(false);
                addToLibrary.gameObject.SetActive(true);               
            
            }
           
        });

        //Ok-Button (Add To A library-Button)
        clonePrefabIportFileMenu.transform.Find("Footer").transform.Find("btn_OK").GetComponent<Button>().onClick.AddListener(() => {

            Library callingLibrary = CommonVariables.selectedLibraryContetnt;

            if (libraryList.Count == 0)
            {
                alertWarning.thereIsNoLibraryToSelect(prefabAlertWarning, canvas);
            }
            if (ImportingFuntions.wordsToCreateNewLibraryOnTheDevice.Count == 0
                || callingLibrary == null)
            {
                alertWarning.pleaseSelectALibrary(prefabAlertWarning, canvas);
            }
            else
            {
                //if the update occurs succefullly
                bool control = Update.addingThelibraryToExistOne(callingLibrary, ImportingFuntions.wordsToCreateNewLibraryOnTheDevice);
               if (control)
                {  
                    //remove the libraries from libraryList because this library is added to a file in device
                    foreach (Library item in ImportingFuntions.wordsToCreateNewLibraryOnTheDevice)
                    {
                        libraryList.Remove(libraryList.Find(element => element.libname == item.name));
                    }

                    //  //removes added (saved) libraries
                    ImportingFuntions.wordsToCreateNewLibraryOnTheDevice.Clear();
                    
                    //reloads rest of the new files
                    createSelectLanguageMenu();
                }                
            }
        });

        //Cancel-button
        clonePrefabIportFileMenu.transform.Find("Header").transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(()=> {
           DestroyImmediate(clonePrefabIportFileMenu);
           SceneManager.LoadScene("MyLibraryNames");
        });

        ImportingFuntions.createLanguageCards(listClonePrefabLanguageCard, libraryList,clonePrefabIportFileMenu, prefabLanguageCard);

        //select-all-button
        clonePrefabIportFileMenu.transform.Find("Header").transform.Find("btn_selectAll").transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
            foreach (GameObject item in listClonePrefabLanguageCard)
            {
                item.transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
            }
        });
        //select none button
        clonePrefabIportFileMenu.transform.Find("Header").transform.Find("btn_selectNone").transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
            foreach (GameObject item in listClonePrefabLanguageCard)
            {
                item.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
            }
        });
    }

   

    void createNewLibrary(string filename, GameObject clonePrefabIportFileMenu)
    {

        if (filename.Length <= 1)
        {
            alertWarning.nullOrEmptyFileName(prefabAlertWarning, clonePrefabIportFileMenu);
        }

        if (PlayerPrefs.GetString(filename) != "")
        {
            alertWarning.ExisitingFileOnTheDirectory(prefabAlertWarning, clonePrefabIportFileMenu);
        }

        if (filename.Length > 1 && filename != null && PlayerPrefs.GetString(filename) == "")
        {
            DestroyImmediate(clonePrefabIportFileMenu);

            List<Word> allwords = new List<Word>();
            List<(Library lib, string libname)> selectedLibraires = new List<(Library lib, string libname)>();
            foreach (Library library in ImportingFuntions.wordsToCreateNewLibraryOnTheDevice)
            {
                foreach (Word word in library.words)
                {
                    allwords.Add(word);
                }
                selectedLibraires.Add((library, library.name));
            }

            // library info is gatherd and saved
            //===========gathering==========
           AllLibrariesInfo libinfo = Create.createAndAddNewLibraryInfo(filename, allwords, selectedLibraires);          

            //===============saving====================
            List<AllLibrariesInfo> allLibrariesInfo = Read.getListAllLibrariesInfo();  
            allLibrariesInfo.Add(libinfo);
            Save.saveListAllLibrariesInfo(allLibrariesInfo);


            // library is created and saved
            //===========creating==========
            Library lib = Create.createNewLibrary(filename, allwords, selectedLibraires, libinfo);
            //Library lib = new Library();
            //lib.name = filename;
            //lib.wordsCount = allwords.Count;
            //lib.words = allwords;
            //if (selectedLibraires.Count == 1)
            //    lib.language = selectedLibraires[0].libname;
            //else
            //    lib.language = "Multilingual";
            //libinfo.wordsCount = allwords.Count;

            //===============saving====================          
            // all library are saved by its name         
            Save.savetheLibrary(lib);
                //PlayerPrefs.SetString(lib.name, JsonConvert.SerializeObject(lib));

            //remove the libraries from libraryList because this library is converted a file in device
            foreach ((Library lib, string libname) item in selectedLibraires)
            {
                libraryList.Remove(libraryList.Find(element => element.libname == item.libname));
            }
            //removes saved libraries
            ImportingFuntions.wordsToCreateNewLibraryOnTheDevice.Clear();

            //reloads rest of the new files
            createSelectLanguageMenu();
        }
    }


    public void pickAndLoadTheFile()
    {
        StartCoroutine(getFileFromDeviceAndCreateNewLibraries());      
    }

   
}

