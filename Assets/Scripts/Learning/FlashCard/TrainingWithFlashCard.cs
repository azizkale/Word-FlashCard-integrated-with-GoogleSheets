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
    int index = 0; // Count of the List<Word> that is learnd runtime
    public GameObject startButton;
    Library libraryToLearn;
    Word displayedWord;
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
        displayedWord = libraryToLearn.words[index];

        turningCard.GetComponent<Image>().transform.Find("btnQuestion").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = displayedWord.theWord;

        turningCard.GetComponent<Image>().transform.Find("btnAnswer").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = displayedWord.meaning;

        //to display sightCount of the word
        wordInfo.transform.Find("SightCountIcon").transform.Find("TMP_SightCount").GetComponent<TextMeshProUGUI>().text = displayedWord.numberOfSight.ToString();

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
        wordInfo.transform.Find("SightCountIcon").transform.Find("TMP_SightCount").GetComponent<TextMeshProUGUI>().text = (++displayedWord.numberOfSight).ToString();

        Save.savetheLibrary(libraryToLearn);

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
        if (index > 0)
        {
            btnnext.interactable = true;
        }
        else
            btnprevious.interactable = false; // if the first learning word is displayed


    }
    private void displayWordNumber(Library lib, int index)
    {
        wordsNumber.text = "["+(index + 1).ToString() + "/" + lib.wordsCount.ToString()+"]";
    }

    private Library sortTheWordList(Library lib)
    {
        //thank to this function user sees always the word which was less seen.
        lib.words.Sort(delegate (Word x, Word y) {
            return x.numberOfSight.CompareTo(y.numberOfSight);
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

            //question text
            TMP_InputField inputQ = cloneprefabupdateword.transform.Find("TMPQuestion").GetComponent<TMP_InputField>();
            inputQ.text = displayedWord.theWord;

            //answer text
            TMP_InputField inputA = cloneprefabupdateword.transform.Find("TMPAnswer").GetComponent<TMP_InputField>();
            inputA.text = displayedWord.meaning;            
            
            //when this is clicked, the text of TMP_InputFileds are updated
            cloneprefabupdateword.transform.Find("btnUpdate").GetComponent<Button>().onClick.AddListener(() => {
                if (!string.IsNullOrEmpty(inputQ.text) && !string.IsNullOrEmpty(inputA.text))
                {
                    Update.updateSingleWord(displayedWord, libraryToLearn, inputQ.text, inputA.text);
                    //chamges the displayWord on the screen
                    displayedWord.theWord = inputQ.text;
                    displayedWord.meaning = inputA.text;
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

            cloneprefabupdateword.transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => { 
                DestroyImmediate(cloneprefabupdateword); 
            });


        });

        cloneSingleWordEditMenu.transform.Find("btnSenToArchive").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("sent to archive"); });

        cloneSingleWordEditMenu.transform.Find("btnReset").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("Reseted"); });

        cloneSingleWordEditMenu.transform.Find("btnDelete").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("Deleted"); });

        cloneSingleWordEditMenu.transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => {
            btnSingleWordEditMenu.gameObject.SetActive(true);
            DestroyImmediate(cloneSingleWordEditMenu); 
        });
       

    }


   
}
