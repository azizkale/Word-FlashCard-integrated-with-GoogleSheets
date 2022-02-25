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
        menuObject.transform.Find("btn_Close").GetComponent<GameObject>().SetActive(true);
    }

   
    IEnumerator menuopen()
    {
        RectTransform rt = (RectTransform)menuObject.transform;
        float width = rt.rect.width;
        float height = rt.rect.height;
        if(height < 51f)
        {
          for (int i = 1; i <= 10; i++)
            {
                rt.sizeDelta = new Vector2(height + 20f * i, width);
                yield return new WaitForSeconds(0.0000001f);
            }
        }
      
    }

    public void closeMenu()
    {
        StartCoroutine(menuclose());
        menuObject.transform.Find("btn_Close").GetComponent<GameObject>().SetActive(false);

    }

    IEnumerator menuclose()
    {
        RectTransform rt = (RectTransform)menuObject.transform;
        float width = rt.rect.width;
        float height = rt.rect.height;

        //for (int i = 1; i <= 10; i++)
        //{
        //    rt.sizeDelta = new Vector2(height - 20f * i, width);
        //    yield return new WaitForSeconds(0.0000001f);
        //}
        yield return rt.sizeDelta = new Vector2(50f, width);
    }
}
