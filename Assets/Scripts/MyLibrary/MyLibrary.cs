using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyLibrary : MonoBehaviour
{
    public GameObject prefabWordsPairCard; 
    public GameObject prefabUpdateWord;
    public GameObject prefabCompleteSuccessfully;
    public GameObject prefabGeneralWarnung;
    public GameObject scrolContainer;
    public GameObject canvas;
    public GameObject libraryTitle;
    public GameObject libraryWordCount;

    public TMP_InputField searchInput;

    private List<GameObject> listPrefabClonesbWordPair = new List<GameObject>();

    public TMP_Dropdown dropdownCallingLibraryOption;
    private Library theLibrary = new Library();
    private List<Word> listToManupulateWords = new List<Word>();   

    void Start()
    {
        createFileNamesCards();       
    }

    public void createFileNamesCards()
    {       
        switch (CommonVariables.callingLibrary.callingCode)
        {
            case CallingCode.all:
                theLibrary = Read.getLibrarysAllWords(CommonVariables.callingLibrary.libraryName);
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 1;
                break;
            case CallingCode.active:
                theLibrary = Read.getLibraryActiveWords(CommonVariables.callingLibrary.libraryName);
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 0;
                break;
            case CallingCode.archive:
                theLibrary = Read.getLibraryArchieveWords(CommonVariables.callingLibrary.libraryName);
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 2;
                break;
            case CallingCode.search:
                theLibrary = Read.getBeingSearchedwordsInASinglelibrary(CommonVariables.callingLibrary.libraryName,searchInput.text);
                break;
            default:
                theLibrary = Read.getLibrarysAllWords(CommonVariables.callingLibrary.libraryName);
                break;
        }

        //word count of the current library theLibrary)
        libraryWordCount.GetComponent<TextMeshProUGUI>().text = theLibrary.words.Count.ToString();
        //theLibrary's name
        libraryTitle.GetComponent<TextMeshProUGUI>().text = CommonVariables.charachterLimit(theLibrary.name, 14);

        resetClones();
        listPrefabClonesbWordPair.Clear();

        createWordPairGameObjects(theLibrary.words);      
    }

    private void createWordPairGameObjects(List<Word> list)
    {
        foreach (Word word in list)
        {
            GameObject cloneprefabWordsPairCard = Instantiate(prefabWordsPairCard, scrolContainer.transform.position, Quaternion.identity, scrolContainer.transform) as GameObject;

            //TMP_Question
            TextMeshProUGUI questionOnPrefab = cloneprefabWordsPairCard.transform.Find("Button").transform.Find("TMP_question").GetComponent<TextMeshProUGUI>();
            questionOnPrefab.text = CommonVariables.charachterLimit(word.theWord, 30);

            //TMP_Answer
            TextMeshProUGUI answerOnPrefab = cloneprefabWordsPairCard.transform.Find("Button").transform.Find("TMP_answer").GetComponent<TextMeshProUGUI>();
            answerOnPrefab.text = CommonVariables.charachterLimit(word.meaning, 30);

            //Toggle on the word-pair ()
            cloneprefabWordsPairCard.transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener((val) => {
                if (val)
                    listToManupulateWords.Add(word);
                if (!val)
                    listToManupulateWords.Remove(word);
            });

            //the word-pair (select BUTTON)
            cloneprefabWordsPairCard.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {

                //Update Prefab
                GameObject cloneprefabupdateword = Instantiate(prefabUpdateWord, canvas.transform.position, Quaternion.identity, canvas.transform);
                cloneprefabupdateword.transform.localPosition = Vector3.zero;

                //view Count info
                TextMeshProUGUI textViewCount = cloneprefabupdateword.transform.Find("TopInfo").transform.Find("ViewCount").GetComponent<TextMeshProUGUI>();
                textViewCount.text = word.viewCount.ToString();

                //language info
                TextMeshProUGUI textLanguage = cloneprefabupdateword.transform.Find("TopInfo").transform.Find("language").GetComponent<TextMeshProUGUI>();
                textLanguage.text = word.languageFrom + "-" + word.languageTo;

                //question text
                TMP_InputField inputQ = cloneprefabupdateword.transform.Find("TextField").transform.Find("TMPQuestion").GetComponent<TMP_InputField>();
                inputQ.text = word.theWord;

                //answer text
                TMP_InputField inputA = cloneprefabupdateword.transform.Find("TextField").transform.Find("TMPAnswer").GetComponent<TMP_InputField>();
                inputA.text = word.meaning;

                //UPDATE button
                cloneprefabupdateword.transform.Find("BottomMenu").transform.Find("btnUpdate").GetComponent<Button>().onClick.AddListener(() => {
                    updateTheWord(word, canvas, cloneprefabupdateword, inputA, inputQ, cloneprefabWordsPairCard);
                });

                //CANCEL button
                cloneprefabupdateword.transform.Find("BottomMenu").transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => {
                    DestroyImmediate(cloneprefabupdateword);
                });

            });
            listPrefabClonesbWordPair.Add(cloneprefabWordsPairCard);
        }
    }

    public void backToMyLibraryNames()
    {
        SceneManager.LoadScene("MyLibraryNames");
    }

    private void updateTheWord(Word word, GameObject canvas, GameObject cloneprefabupdateword, TMP_InputField inputA, TMP_InputField inputQ, GameObject cloneprefabWordsPairCard)
    {
        if (!string.IsNullOrEmpty(inputQ.text) && !string.IsNullOrEmpty(inputA.text))
        {
            Update.updateSingleWord(word, theLibrary, inputQ.text, inputA.text); 
            DestroyImmediate(cloneprefabupdateword);
            alertWarning.completedSuccesfully(prefabCompleteSuccessfully, canvas);

            //renew the on the screen
            cloneprefabWordsPairCard.transform.Find("Button").transform.Find("TMP_question").GetComponent<TextMeshProUGUI>().text = inputQ.text;
            
            cloneprefabWordsPairCard.transform.Find("Button").transform.Find("TMP_answer").GetComponent<TextMeshProUGUI>().text = inputA.text;
        }
        else
            alertWarning.generalWarning(prefabGeneralWarnung, canvas, "Question and answer fields cannot be left blank!");
    }

    public void deleteTheSelectedWords()
    {
        foreach (Word word in listToManupulateWords)
        {
            //theLibrary is re-filled in oreder to save all words except deleted one
            theLibrary = Read.getLibrarysAllWords(theLibrary.name);
            Delete.deleteSingleWord(word, theLibrary);
        }
        //reload the scene after deleting
        SceneManager.LoadScene("LibraryContent");
    }

    public void sendToArchive()
    {
        foreach (Word word in listToManupulateWords)
        {          
            //theLibrary is re-filled in oreder to save all words except deleted one
            theLibrary = Read.getLibrarysAllWords(theLibrary.name);
            //changing "archive" property oft the selected word
            theLibrary.words.Find(w => w.theWord == word.theWord && w.meaning == word.meaning).archive = true;

            Save.saveSingleLibrary(theLibrary);// resave
        }
        //reload the scene after deleting
        SceneManager.LoadScene("LibraryContent");
    }

    public void makeActive()
    {
        foreach (Word word in listToManupulateWords)
        {
            //theLibrary is re-filled in oreder to save all words except deleted one
            theLibrary = Read.getLibrarysAllWords(theLibrary.name);
            //changing "archive" property oft the selected word
            theLibrary.words.Find(w => w.theWord == word.theWord && w.meaning == word.meaning).archive = false;

            Save.saveSingleLibrary(theLibrary);// resave
        }
        //reload the scene after deleting
        SceneManager.LoadScene("LibraryContent");
    }

    public void optionalLibraryContent()
    {       
        int menuIndex = dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value;
        //reloasd the words list (theLibary.words)
        switch (menuIndex)
        {
            case 1:
                CommonVariables.callingLibrary = (theLibrary.name, CallingCode.all);
                break;  
            case 0:
                CommonVariables.callingLibrary = (theLibrary.name, CallingCode.active);
                break;
            case 2:
                CommonVariables.callingLibrary = (theLibrary.name, CallingCode.archive);
                break;
        }
        createFileNamesCards();       
    }

   public void selectAllWords()
    {
        foreach (GameObject clone in listPrefabClonesbWordPair)
        {
            clone.transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
        }
    }

    public void unselectAllWords()
    {
        foreach (GameObject clone in listPrefabClonesbWordPair)
        {
            clone.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
        }
        listToManupulateWords.Clear();
    }
   
    private void resetClones()
    {
        foreach (GameObject clone in listPrefabClonesbWordPair)
        {
            DestroyImmediate(clone);
        }
    }

    public void searchWord()
    {
        //while searching the app display all words in the dropdown
        dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 1;

        CommonVariables.callingLibrary = (theLibrary.name, CallingCode.search);

        createFileNamesCards();
    }
}