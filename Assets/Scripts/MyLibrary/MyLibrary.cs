using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyLibrary : MonoBehaviour
{
    public GameObject wordsPairCard; //prefab
    public GameObject scrolContainer;
    GameObject clone;

    TextMeshProUGUI cardQuestion;
    TextMeshProUGUI cardAnswer;
    public TextMeshProUGUI sceneTitle;
    void Start()
    {
        createFileNamesCards();
    }

    public void createFileNamesCards()
    {

        Library theLibrary = Read.getLibraryContentWithoutArchive(CommonVariables.libraryName);
            //JsonConvert.DeserializeObject<Library>(PlayerPrefs.GetString(CommonVariables.libraryName));

        foreach (Word word in theLibrary.words)
        {
            clone = Instantiate(wordsPairCard, scrolContainer.transform.position, Quaternion.identity, scrolContainer.transform) as GameObject;

            cardQuestion = clone.transform.Find("Button").transform.Find("TMP_question").GetComponent<TextMeshProUGUI>();
            cardQuestion.text = CommonVariables.charachterLimit(word.theWord,30);
            cardAnswer = clone.transform.Find("Button").transform.Find("TMP_answer").GetComponent<TextMeshProUGUI>();
            cardAnswer.text = CommonVariables.charachterLimit(word.meaning,30);
        }
    }

    public void backToMyLibraryNames()
    {
        SceneManager.LoadScene("MyLibraryNames");
    }

    public void startLearning()
    {

    }
}
