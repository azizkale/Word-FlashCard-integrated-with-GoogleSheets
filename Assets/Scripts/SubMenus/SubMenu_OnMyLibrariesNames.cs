using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubMenu_OnMyLibrariesNames : MonoBehaviour
{
   public static void rename(GameObject prefabsubmenu_renamefunction, GameObject clonesubmenu,GameObject prefabalertwarning, AllLibrariesInfo lab, GameObject canvas)
   {
        GameObject cloneRename = Instantiate(
            prefabsubmenu_renamefunction,
            clonesubmenu.transform.position,
           Quaternion.identity,
           clonesubmenu.transform);
        cloneRename.transform.localScale = Vector3.one;

        cloneRename.transform.Find("InputField (TMP)").transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TextMeshProUGUI>().text = lab.name;

        //Cancel-Button on "rename-function" card (prefab) 
        cloneRename.transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(() => {
            DestroyImmediate(cloneRename);
        });

        // OK-Button  on "rename-function" card (prefab) 
        cloneRename.transform.Find("btn_OK").GetComponent<Button>().onClick.AddListener(() => {

            string newname = cloneRename.transform.Find("InputField (TMP)").transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>().text;

            if (newname.Length <= 1)
            {
                alertWarning.nullOrEmptyFileName(prefabalertwarning, canvas);
            }

            if (PlayerPrefs.GetString(newname) != "")
            {
                alertWarning.ExisitingFileOnTheDirectory(prefabalertwarning, canvas);
            }


            if (PlayerPrefs.GetString(newname) == "" && newname.Length > 1 && newname != null)
            {               
                //and re-saved the library by its new name (newname)
                Update.renameLibrary(lab, newname);

                //closes submenu
                DestroyImmediate(clonesubmenu);

                //reloads to scene to reload the libraries with the new name
                SceneManager.LoadScene("MyLibraryNames");
            }
        });



        

    }

   public static void delete(AllLibrariesInfo singlelibraryInfo, GameObject canvas, GameObject prefabDeleteCard)
    {
        GameObject cloneDeleteCard = Instantiate(prefabDeleteCard, canvas.transform.position, Quaternion.identity, canvas.transform);
        cloneDeleteCard.transform.localScale = Vector3.one;     

        TextMeshProUGUI libraryname = cloneDeleteCard.transform.Find("InputField (TMP)").transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>();

        cloneDeleteCard.transform.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(() => {
            DestroyImmediate(cloneDeleteCard);
        });
            cloneDeleteCard.transform.Find("btn_OK").GetComponent<Button>().onClick.AddListener(() => {
           
                //at first info about library are deleted and re-saved
                Delete.deleteFromAllLibrariesInfo(singlelibraryInfo);

                // then the library deleted from device
                Delete.deleteLibraryFromDevice(singlelibraryInfo);

                //reloads to scene to reload the libraries with the new name
                SceneManager.LoadScene("MyLibraryNames");
           
        });

       
    }


}
