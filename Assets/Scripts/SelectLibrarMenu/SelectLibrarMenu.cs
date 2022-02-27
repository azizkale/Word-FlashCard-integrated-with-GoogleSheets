using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelectLibrarMenu : MonoBehaviour
{    
 
    
    void Start()
    {
       
    }

    //creates the "selectlibrary-menu" and returns the librray which in clicked button
    //and fill the variable " CommonVariables.selectedLibraryContetnt" int the class CommonVariables
    public static Library createSelectMenu(GameObject canvas, GameObject prefabselectLibraryBackground, GameObject prefabselectLibraryButton)
    {
        CommonVariables.selectedLibraryContetnt = null;
        List<AllLibrariesInfo> allLibrariesInfo = Read.getListAllLibrariesInfo();

        GameObject cloneselectLibraryBackground = Instantiate(prefabselectLibraryBackground, canvas.transform.position, Quaternion.identity) as GameObject;
        cloneselectLibraryBackground.transform.SetParent(canvas.transform);
        cloneselectLibraryBackground.transform.localScale = Vector3.one;

        //cancel-button
        cloneselectLibraryBackground.transform.Find("Header").transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(() => {
            DestroyImmediate(cloneselectLibraryBackground);
        });

        foreach (AllLibrariesInfo lab in allLibrariesInfo)
        {
            GameObject cloneSelectLibraryButton = Instantiate(
                prefabselectLibraryButton,
              cloneselectLibraryBackground.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").transform.position, 
                Quaternion.identity,
                cloneselectLibraryBackground.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").transform);
            cloneselectLibraryBackground.transform.localScale = Vector3.one;

            //Library names
            cloneSelectLibraryButton.transform.Find("Button").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = CommonVariables.charachterLimit(lab.name,10);
          

            // button selects the library which is wanted to learn
           cloneSelectLibraryButton.transform.Find("Button").GetComponent<Button>()
            .onClick.AddListener(() => {

                CommonVariables.selectedLibraryContetnt = Read.getLibraryActiveWords(lab.name);
                DestroyImmediate(cloneselectLibraryBackground); // to cancel select library menu

            });
        }
        return CommonVariables.selectedLibraryContetnt;
    }    
   
}
