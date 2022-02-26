using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JsonData : MonoBehaviour
{
    public List<Word> wordList = new List<Word>();// the words that came from Google
    //wordList is converted to libraries with their names
    private List<(Library lib, string libname)> libraryList = new List<(Library lib, string libname)>();
    //according to the count of different languages in the importing file, it is created language-cards from prefabLanguageCard
    List<GameObject> listClonePrefabLanguageCard = new List<GameObject>();

    public GameObject prefabIportFileMenu;
    public GameObject prefabGetUrl;
    public GameObject prefabGiveName;
    public GameObject prefabAlertWarning;
    public GameObject prefabselectLibraryBackground;
    public GameObject prefabselectLibraryButton;
    public GameObject prefabLanguageCard;
    public GameObject prefabLoading;
    public GameObject canvas;

    GameObject clonePrefabLoading;
    void Start()
    {       

    }

    public void getDataFromGoogle()
    {
        Loading();
        string url = "";
        //givename-card to the creating file
        GameObject clonePrefabGetUrl = Instantiate(prefabGetUrl, canvas.transform.position, Quaternion.identity, canvas.transform) as GameObject;

        //OK button on the prefabGiveName
        clonePrefabGetUrl.transform.Find("btn_OK").GetComponent<Button>().onClick.AddListener(() => {

            string sheetLink = clonePrefabGetUrl.transform.Find("InputField (TMP)").transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>().text;

            //to get google-sheet id
            string[] seperatedLink = sheetLink.Split('/');

            url = "http://docs.google.com/spreadsheets/d/"+ seperatedLink[5]+"/gviz/tq?";


            // A correct website page.
            StartCoroutine(GetRequest(url));

            DestroyImmediate(clonePrefabGetUrl);           
        });

        //CANCEL button on the prefabGiveName
        clonePrefabGetUrl.transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(() => {
            DestroyImmediate(clonePrefabGetUrl);
        });

    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = null;
       
        webRequest = UnityWebRequest.Get(uri);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:

            string jsonString = "";
            string str = webRequest.downloadHandler.text.Substring(47);

            for (int i = 0; i < str.Length - 2; i++)
            {
                jsonString += str[i];
            }                      

            JObject json = JObject.Parse(jsonString);

            JArray rows = (JArray)json["table"]["rows"];

                      
            foreach (var item in rows)
            {
                Word word = new Word();
                word.languageFrom = item["c"][0]["v"].ToString();
                word.languageTo = item["c"][1]["v"].ToString();
                word.theWord = item["c"][2]["v"].ToString();
                word.meaning = item["c"][3]["v"].ToString();

                wordList.Add(word);
                            
            }
            break;
        }
        
        StartCoroutine(createlibrariesListByUsingImportedFile());
        createSelectLanguageMenu();

        StopAllCoroutines();
    }

    IEnumerator createlibrariesListByUsingImportedFile()
    {
        foreach (Word word in wordList)
        {
            string name = word.languageFrom + "-" + word.languageTo;
            if (libraryList.Contains(libraryList.Find(li => li.libname == name))) // if the libarary exist in the list libraries
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
        yield return libraryList;
    }

    void createSelectLanguageMenu()
    {
        GameObject clonePrefabIportFileMenu = Instantiate(prefabIportFileMenu, canvas.transform.position, Quaternion.identity, canvas.transform) as GameObject;
        //
        clonePrefabIportFileMenu.transform.Find("Scroll View").transform.position = canvas.transform.position;
        DestroyImmediate(clonePrefabLoading);

        //CreateNewLibrary-button
        clonePrefabIportFileMenu.transform.Find("Footer").transform.Find("btn_CreateNewLibrary").GetComponent<Button>().onClick.AddListener(() => {
            if (libraryList.Count > 0 && ImportingFuntions.wordsToCreateNewLibraryOnTheDevice.Count > 0)
            {
                //givename-card to the creating file
               GameObject clonePrefabGiveName = Instantiate(prefabGiveName, canvas.transform.position, Quaternion.identity, canvas.transform) as GameObject;
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
        clonePrefabIportFileMenu.transform.Find("Footer").transform.Find("btn_OpenTheSelectLibraryMenu").GetComponent<Button>().onClick.AddListener(() => {
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
        clonePrefabIportFileMenu.transform.Find("Header").transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(() => {
            DestroyImmediate(clonePrefabIportFileMenu);
            SceneManager.LoadScene("MyLibraryNames");
        });

        ImportingFuntions.createLanguageCards(listClonePrefabLanguageCard, libraryList, clonePrefabIportFileMenu, prefabLanguageCard);

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


            //===============saving====================          
            // all library are saved by its name         
            Save.saveSingleLibrary(lib);
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

    private void Loading()
    {
        clonePrefabLoading = Instantiate(prefabLoading, canvas.transform.position, Quaternion.identity, canvas.transform) as GameObject;
    }
}


