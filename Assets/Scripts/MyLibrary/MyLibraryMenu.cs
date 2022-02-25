using System.Collections;
using UnityEngine;

public class MyLibraryMenu : MonoBehaviour
{
    public GameObject menuObject;
    void Start()
    {
        
    }

    public void openMenu()
    {
        StartCoroutine(menuopen());
        
    }

    IEnumerator menuopen()
    {
        RectTransform rt = (RectTransform)menuObject.transform;

        float width = rt.rect.width;
        float height = rt.rect.height;
       
        for (int i = 0; i < 10; i++)
        {
            rt.sizeDelta = new Vector2(100 + 20f * i, width);
            yield return new WaitForSeconds(0.0000001f);
        }
       
    }
}
