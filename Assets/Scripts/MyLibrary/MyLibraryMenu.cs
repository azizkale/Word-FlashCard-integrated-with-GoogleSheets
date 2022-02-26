using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MyLibraryMenu : MonoBehaviour
{
    public GameObject menuObject;
    void Start()
    {
       
    }

    public void openMenu()
    {
        StartCoroutine(menuopen());      
        menuObject.transform.Find("btn_Close").gameObject.SetActive(true);
    }

    IEnumerator menuopen()
    {
        // local position of the bottom menu (menuObject)
        Vector3 vec = menuObject.transform.localPosition;
        // height of the bottom menu (menuObject)
        RectTransform rt = (RectTransform)menuObject.transform;
        float height = rt.rect.height;
        //height of the open button on the bottom menu
        RectTransform rtbtn = (RectTransform)menuObject.transform.Find("btn_Open").transform;
        float heightbtn = rtbtn.rect.height;

        float theValThatAddedToVecY = height - heightbtn;

        for (int i = 0; i <= theValThatAddedToVecY; i = i + 40)
        {
            menuObject.transform.localPosition = new Vector3(0, vec.y + i, 0);
            yield return new WaitForSeconds(0.000001f);
        }
        menuObject.transform.localPosition = new Vector3(0, vec.y + theValThatAddedToVecY, 0);



    }

    public void closeMenu()
    {
        StartCoroutine(menuclose());
        menuObject.transform.Find("btn_Close").gameObject.SetActive(false);
    }

    IEnumerator menuclose()
    {
        // local position of the bottom menu (menuObject)
        Vector3 vec = menuObject.transform.localPosition;
        // height of the bottom menu (menuObject)
        RectTransform rt = (RectTransform)menuObject.transform;
        float height = rt.rect.height;
        //height of the open button on the bottom menu
        RectTransform rtbtn = (RectTransform)menuObject.transform.Find("btn_Open").transform;
        float heightbtn = rtbtn.rect.height;

        float theValThatAddedToVecY = -height + heightbtn;

        for (float i = 0; i > theValThatAddedToVecY; i = i - 40)
        {
            menuObject.transform.localPosition = new Vector3(0, vec.y + i, 0);
            yield return new WaitForSeconds(0.000001f);
        }
        yield return menuObject.transform.localPosition = new Vector3(0, vec.y + theValThatAddedToVecY, 0);
    }
}
