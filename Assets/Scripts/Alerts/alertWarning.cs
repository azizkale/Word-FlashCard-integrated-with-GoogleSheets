using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class alertWarning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    static void createWarningAlert(GameObject go, GameObject canvas, string warningNote)
    {
        GameObject cloneGo = Instantiate(go, canvas.transform.position, Quaternion.identity, canvas.transform);
        cloneGo.transform.Find("Body").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = warningNote;
        cloneGo.transform.Find("Body").transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => { DestroyImmediate(cloneGo); });
    }

    public static void ExisitingFileOnTheDirectory (GameObject go, GameObject canvas)
    {
        createWarningAlert(go, canvas, "There is already a file with the same name. Please try again with another name!");        
    }

    public static void nullOrEmptyFileName(GameObject go, GameObject canvas)
    {
        createWarningAlert(go, canvas, "Please try again with valid file name");       
    }

    public static void thereIsNoLibraryToSelect(GameObject go, GameObject canvas)
    {
        createWarningAlert(go, canvas, "There is no library to select!");
    }

    public static void pleaseSelectALibrary(GameObject go, GameObject canvas)
    {
        createWarningAlert(go, canvas, "At least one option should be select!");
    }

    public static void completedSuccesfully(GameObject go, GameObject canvas)
    {
        GameObject cloneGo = Instantiate(go, canvas.transform.position, Quaternion.identity, canvas.transform);
        
        cloneGo.transform.Find("Body").transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => { DestroyImmediate(cloneGo); });
    }

    public static void generalWarning(GameObject go, GameObject canvas, string warningText)
    {
        GameObject cloneGo = Instantiate(go, canvas.transform.position, Quaternion.identity, canvas.transform);
        cloneGo.transform.Find("Body").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = warningText;
        cloneGo.transform.Find("Body").transform.Find("btnCancel").GetComponent<Button>().onClick.AddListener(() => { DestroyImmediate(cloneGo); });
    }
}
