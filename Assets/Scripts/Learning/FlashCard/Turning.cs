using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Turning : MonoBehaviour
{
    public GameObject turningCard;
    public Button btnBack;
    public Button btnFront;
    public Button btnNext;
    public Button btnPrevious;
    public Button btnCorrectAnswer;
    public Button btnWrongAnswer;
    void Start()
    {     
       
    }

    

   IEnumerator turnTheCardToBack(GameObject go)
    {
        if (go.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            for (int i = 0; i <= 5; i++)
            {
                go.transform.rotation = Quaternion.Euler(0, 36*i, 0);
                yield return new WaitForSeconds(0.00001f);
            }
        }
        btnBack.gameObject.SetActive(false);
        btnFront.gameObject.SetActive(true);
        btnCorrectAnswer.gameObject.SetActive(true);
        btnWrongAnswer.gameObject.SetActive(true);
    }
   
   IEnumerator turnTheTheCardToFront(GameObject go)
    {
        for (int i = 5; i >= 0; i--)
        {
            turningCard.transform.rotation = Quaternion.Euler(0, 36*i, 0);
            yield return new WaitForSeconds(0.0000000000001f);
        }
        btnBack.gameObject.SetActive(true);
        btnFront.gameObject.SetActive(false);
        btnCorrectAnswer.gameObject.SetActive(false);
        btnWrongAnswer.gameObject.SetActive(false);      
    }

    public void turnToBack()
    {       
        StartCoroutine(turnTheCardToBack(turningCard));
        btnNext.gameObject.SetActive(false);
        btnPrevious.gameObject.SetActive(false);
    }
    public void turnToFront()
    {        
        StartCoroutine(turnTheTheCardToFront(turningCard));
        btnNext.gameObject.SetActive(true);
        btnPrevious.gameObject.SetActive(true);
    }
    public void backToMyLibraryNames()
    {
        SceneManager.LoadScene("MyLibraryNames");
    }
}
