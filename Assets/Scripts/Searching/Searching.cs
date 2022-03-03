using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Searching : MonoBehaviour
{
    public GameObject prefabWordsPairCard;
    public GameObject prefabUpdateWord;
    public GameObject prefabCompleteSuccessfully;
    public GameObject prefabGeneralWarnung;
    public GameObject scrolContainer;
    public GameObject canvas;
    public GameObject libraryWordCount;
    public TextMeshProUGUI libraryTitle;

    public TMP_InputField searchInput;

    private List<GameObject> listPrefabClonesbWordPair = new List<GameObject>();

    public TMP_Dropdown dropdownCallingLibraryOption;
    private Library theLibrary = new Library();
    private List<Word> listToManupulateWords = new List<Word>();
    void Start()
    {
        createWordsCards();
    }

    public void createWordsCards()
    {
        switch (CommonVariables.callingLibrary.callingCode)
        {
            case CallingCode.all:
                theLibrary = Read.getAllLibrariesWords();
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 1;
                break;
            case CallingCode.active:
                theLibrary = Read.getAllLibrariesActiveWords();
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 0;
                break;
            case CallingCode.archive:
                theLibrary = Read.getAllLibrariesArchiveWords();
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 2;
                break;
            case CallingCode.search:
                theLibrary = Read.getBeingSearchWordsInAlllibraries(CommonVariables.callingLibrary.libraryName, searchInput.text);
                break;
            default:
                theLibrary = Read.getAllLibrariesWords();
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 1;
                break;
        }

        //word count of the current library theLibrary)
        libraryWordCount.GetComponent<TextMeshProUGUI>().text = theLibrary.words.Count.ToString();
        //theLibrary's name
        libraryTitle.GetComponent<TextMeshProUGUI>().text = theLibrary.name;

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
            questionOnPrefab.text = word.theWord;

            //TMP_Answer
            TextMeshProUGUI answerOnPrefab = cloneprefabWordsPairCard.transform.Find("Button").transform.Find("TMP_answer").GetComponent<TextMeshProUGUI>();
            answerOnPrefab.text = word.meaning;

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
            //to get the library that being updated word belongs
            Library lib = Read.getLibrarysAllWords(word.libraryName);

            Update.updateSingleWord(word, lib, inputQ.text, inputA.text);
            DestroyImmediate(cloneprefabupdateword);
            alertWarning.completedSuccesfully(prefabCompleteSuccessfully, canvas);

            //renew the on the screen
            word.theWord = inputQ.text;
            word.meaning = inputA.text;

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
            Library beingDeletedWordsLibrary = Read.getLibrarysAllWords(word.libraryName);
            Delete.deleteSingleWord(word, beingDeletedWordsLibrary);
        }
        //reload the scene after deleting
        SceneManager.LoadScene("Searching");
    }

    public void sendToArchiveTheSelectedWords()
    {
        foreach (Word word in listToManupulateWords)
        {
            //theLibrary is re-filled in oreder to save all words except deleted one
           Library beingUpdatedWordsLibrary = Read.getLibrarysAllWords(word.libraryName);
            //changing "archive" property oft the selected word
            beingUpdatedWordsLibrary.words.Find(w => w.theWord == word.theWord && w.meaning == word.meaning).archive = true;

            Save.saveSingleLibrary(beingUpdatedWordsLibrary);// resave
        }
        //reload the scene after deleting
        SceneManager.LoadScene("Searching");
    }

    public void makeActiveTheSelectedWords()
    {
        foreach (Word word in listToManupulateWords)
        {
            //theLibrary is re-filled in oreder to save all words except deleted one
            Library beingMadeActivedWordsLibrary = Read.getLibrarysAllWords(word.libraryName);
            //changing "archive" property oft the selected word
            beingMadeActivedWordsLibrary.words.Find(w => w.theWord == word.theWord && w.meaning == word.meaning).archive = false;

            Save.saveSingleLibrary(beingMadeActivedWordsLibrary);// resave
        }
        //reload the scene after deleting
        SceneManager.LoadScene("Searching");
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
        createWordsCards();
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
        switch (theLibrary.name)
        {
            case "All Words":
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 1;
                break;
            case "All Active Words":
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 0;
                break;
            case "All Archive Words":
                dropdownCallingLibraryOption.GetComponent<TMP_Dropdown>().value = 2;
                break;
        }
       
        CommonVariables.callingLibrary = (theLibrary.name, CallingCode.search);

        createWordsCards();
    }


}
