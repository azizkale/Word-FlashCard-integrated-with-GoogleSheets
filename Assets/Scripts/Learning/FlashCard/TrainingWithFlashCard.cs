using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingWithFlashCard : MonoBehaviour
{
    //to open select-library-menu
    public GameObject canvas; // to append Select Menu to canvas

    //to append library-name-cards to prefabselectLibrary
    public GameObject prefabselectLibraryBackground;
    public GameObject prefabselectLibraryButton;
    //===============================================================

    public Image turningCard;
    private int index = 0; // Count of the List<Word> that is learnd runtime
    public GameObject startButton;
    private Library libraryToLearn;
    private Word displayingWord;
    public GameObject wordInfo;
    public Button btnCorrectAnswer;
    public Button btnWrongAnswer;
    public Button btnNext;
    public Button btnPrevious;
    public TextMeshProUGUI wordsNumber;
    //
    public GameObject btnSingleWordEditMenu;
    public GameObject singleWordEditMenu;
    //
    public GameObject prefabUpdateWord;
    public GameObject prefabCompleteSuccessfully;
    public GameObject prefabGeneralWarnung;
    public GameObject prefabGeneralCompleted;

    void Start()
    {
        //picks the library content up by showing "SelectLibrary" menu
        //Library and its content goes to  CommonVariables.selectedLibraryContetnt
        SelectLibrarMenu.createSelectMenu(canvas, prefabselectLibraryBackground, prefabselectLibraryButton);

       
    }

    public void startTurningFlashCardLearning()
    {
        libraryToLearn = sortTheWordList(CommonVariables.selectedLibraryContetnt);
        startButton.gameObject.SetActive(false);
        showTheWordsOnTheTurningCard(0);
        displayWordNumber(libraryToLearn, index);
        //edit button displays just when the flashCard starts
        btnSingleWordEditMenu.SetActive(true);
    }

    private void showTheWordsOnTheTurningCard(int indx)
    {
        displayingWord = libraryToLearn.words[index];

        turningCard.GetComponent<Image>().transform.Find("btnQuestion").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = displayingWord.theWord;

        turningCard.GetComponent<Image>().transform.Find("btnAnswer").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = displayingWord.meaning;

        //to display sightCount of the word
        wordInfo.transform.Find("SightCountIcon").transform.Find("TMP_SightCount").GetComponent<TextMeshProUGUI>().text = displayingWord.viewCount.ToString();

        //to control that user cliks it only one time
        btnCorrectAnswer.GetComponent<Button>().interactable = true;
    }

    public void nextWord()
    {
        if (index < libraryToLearn.wordsCount - 1)
        {
            index++;
            showTheWordsOnTheTurningCard(index);           
        }

        btnNextInteractablitiyControl(btnNext,btnPrevious,index);
        displayWordNumber(libraryToLearn, index);
    }

    public void previousWord()
    {
        if (index > 0)
        {
            index--;
            showTheWordsOnTheTurningCard(index);
        }
        btnPreviuosInteractablitiyControl(btnNext, btnPrevious, index);
        displayWordNumber(libraryToLearn, index);
    }

   public void correctAnswer()
    {      
        //increases and displays the count of sight of the word
        wordInfo.transform.Find("SightCountIcon").transform.Find("TMP_SightCount").GetComponent<TextMeshProUGUI>().text = (++displayingWord.viewCount).ToString();

        Save.saveSingleLibrary(libraryToLearn);

        //to control that user cliks it only one time
       btnCorrectAnswer.GetComponent<Button>().interactable = false;
    }

    public void wrongAnswer()
    {
        Debug.Log("wrong answer");
    }

    private void btnNextInteractablitiyControl(Button btnnext, Button btnprevious, int index)
    {
        if (index < libraryToLearn.words.Count - 1)
        {
            btnnext.interactable = true;
            btnprevious.interactable = true;
        }

        else
        {
            btnnext.interactable = false; // if the last learning word is displayed
        }
    }

    private void btnPreviuosInteractablitiyControl(Button btnnext, Button btnprevious, int index)
    {
        if (index >= 0)
        {
            btnnext.interactable = true;
        }
        else
            btnprevious.interactable = false; // if the first learning word is displayed


    }
    private void displayWordNumber(Library lib, int index)
    {
        wordsNumber.text = "["+(index + 1).ToString() + "/" + lib.words.Count.ToString()+"]";
    }

    private Library sortTheWordList(Library lib)
    {
        //thank to this function user sees always the word which was less seen.
        lib.words.Sort(delegate (Word x, Word y) {
            return x.viewCount.CompareTo(y.viewCount);
        });

        return lib;
    }

    public void closeBottomMenu()
    {
        DestroyImmediate(singleWordEditMenu);
    }

    public void showSingelWordEditmMenu()
    {
        btnSingleWordEditMenu.gameObject.SetActive(false);
        GameObject cloneSingleWordEditMenu = Instantiate(
            singleWordEditMenu,
            canvas.transform.position,
            Quaternion.identity,
            canvas.transform);
        cloneSingleWordEditMenu.transform.localPosition = Vector3.zero;

        cloneSingleWordEditMenu.transform.Find("btnUpdateTheWord").GetComponent<Button>().onClick.AddListener(() => {

            GameObject cloneprefabupdateword = Instantiate(prefabUpdateWord, canvas.transform.position, Quaternion.identity, canvas.transform);
            cloneprefabupdateword.transform.localPosition = Vector3.zero;

            //view Count info
            TextMeshProUGUI textViewCount = cloneprefabupdateword.transform.Find("TopInfo").transform.Find("ViewCount").GetComponent<TextMeshProUGUI>();
            textViewCount.text = displayingWord.viewCount.ToString();

            //language info
            TextMeshProUGUI textLanguage = cloneprefabupdateword.transform.Find("TopInfo").transform.Find("language").GetComponent<TextMeshProUGUI>();
            textLanguage.text = displayingWord.languageFrom + "-" + displayingWord.languageTo;

            //question text
            TMP_InputField inputQ = cloneprefabupdateword.transform.Find("TextField").transform.Find("TMPQuestion").GetComponent<TMP_InputField>();
            inputQ.text = displayingWord.theWord;

            //answer text
            TMP_InputField inputA = cloneprefabupdateword.transform.Find("TextField").transform.Find("TMPAnswer").GetComponent<TMP_InputField>();
            inputA.text = displayingWord.meaning;            
            
            //when this is clicked, the text of TMP_InputFileds are updated
            cloneprefabupdateword.transform.Find("BottomMenu").transform.Find("btnUpdate").GetComponent<Button>().onClick.AddListener(() => {
                if (!string.IsNullOrEmpty(inputQ.text) && !string.IsNullOrEmpty(inputA.text))
                {
                    Update.updateSingleWord(displayingWord, libraryToLearn, inputQ.text, inputA.text);
                    //chamges the displayWord on the screen
                    displayingWord.theWord = inputQ.text;
                    displayingWord.meaning = inputA.text;
                    //to reload
                    nextWord();
                    previousWord();

                    DestroyImmediate(cloneprefabupdateword);
                    DestroyImmediate(cloneSingleWordEditMenu);
                    btnSingleWordEditMenu.gameObject.SetActive(true);
                    alertWarning.completedSuccesfully(prefabCompleteSuccessfully, canvas);
                    
                   
                }
                else
                    alertWarning.generalWarning(prefabGeneralWarnung, canvas, "Question and answer fields cannot be left blank!");                
            });

            cloneprefabupdateword.transform.Find("BottomMenu").transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => { 
                DestroyImmediate(cloneprefabupdateword);
                DestroyImmediate(cloneSingleWordEditMenu);
                btnSingleWordEditMenu.gameObject.SetActive(true);
            });
        });

        cloneSingleWordEditMenu.transform.Find("btnSendToArchive").GetComponent<Button>().onClick.AddListener(() => {

            displayingWord.archive = true;          
            libraryToLearn.words.Remove(displayingWord);//remove from screen           
            Save.saveSingleLibrary(libraryToLearn);// resave

            alertWarning.generalWarning(prefabGeneralCompleted, canvas, "The Word was successfully sent to archive.");
            //renew the count of the librarToLearn on the screen
            nextWord();
            previousWord();
            //close the menu
            DestroyImmediate(cloneSingleWordEditMenu);
            btnSingleWordEditMenu.gameObject.SetActive(true);
        });

        cloneSingleWordEditMenu.transform.Find("btnReset").GetComponent<Button>().onClick.AddListener(() => {
            displayingWord.viewCount = 0;
            Save.saveSingleLibrary(libraryToLearn);
            //to reload
            nextWord();
            previousWord();
            alertWarning.generalCompleted(prefabGeneralCompleted, canvas, "The word was successfully reset.");
            DestroyImmediate(cloneSingleWordEditMenu);
            btnSingleWordEditMenu.gameObject.SetActive(true);
        });

        cloneSingleWordEditMenu.transform.Find("btnDelete").GetComponent<Button>().onClick.AddListener(() => {

            libraryToLearn.words.Remove(displayingWord);
            Save.saveSingleLibrary(libraryToLearn);
            //to reload
            nextWord();
            previousWord();
            alertWarning.generalCompleted(prefabGeneralCompleted, canvas, "The word was successfully deleted.");
            DestroyImmediate(cloneSingleWordEditMenu);
            btnSingleWordEditMenu.gameObject.SetActive(true);


        });

        cloneSingleWordEditMenu.transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => {
            btnSingleWordEditMenu.gameObject.SetActive(true);
            DestroyImmediate(cloneSingleWordEditMenu); 
        });
       

    }


   
}
