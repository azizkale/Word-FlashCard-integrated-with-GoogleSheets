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

    TextMeshProUGUI cardQuestion;
    TextMeshProUGUI cardAnswer;
    //public TextMeshProUGUI sceneTitle;
    private Library theLibrary;
    void Start()
    {
        createFileNamesCards();
    }

    public void createFileNamesCards()
    {

        theLibrary = Read.getLibraryContentWithoutArchive(CommonVariables.libraryName);           

        foreach (Word word in theLibrary.words)
        {
           GameObject clone = Instantiate(prefabWordsPairCard, scrolContainer.transform.position, Quaternion.identity, scrolContainer.transform) as GameObject;

            cardQuestion = clone.transform.Find("Button").transform.Find("TMP_question").GetComponent<TextMeshProUGUI>();
            cardQuestion.text = CommonVariables.charachterLimit(word.theWord,30);

            cardAnswer = clone.transform.Find("Button").transform.Find("TMP_answer").GetComponent<TextMeshProUGUI>();
            cardAnswer.text = CommonVariables.charachterLimit(word.meaning,30);

            clone.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
                updateTheWord(word, canvas);
            });
        }
    }

    public void backToMyLibraryNames()
    {
        SceneManager.LoadScene("MyLibraryNames");
    }

    public void startLearning()
    {

    }

    private void updateTheWord(Word word, GameObject canvas)
    {
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

        //when this is clicked, the text of TMP_InputFileds are updated
        cloneprefabupdateword.transform.Find("BottomMenu").transform.Find("btnUpdate").GetComponent<Button>().onClick.AddListener(() => {
            if (!string.IsNullOrEmpty(inputQ.text) && !string.IsNullOrEmpty(inputA.text))
            {
                Update.updateSingleWord(word, theLibrary, inputQ.text, inputA.text);
                //chamges the displayWord on the screen
                word.theWord = inputQ.text;
                word.meaning = inputA.text;              

                DestroyImmediate(cloneprefabupdateword);
                alertWarning.completedSuccesfully(prefabCompleteSuccessfully, canvas);


            }
            else
                alertWarning.generalWarning(prefabGeneralWarnung, canvas, "Question and answer fields cannot be left blank!");
        });

        cloneprefabupdateword.transform.Find("BottomMenu").transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => {
            DestroyImmediate(cloneprefabupdateword);
        });
    }
}
