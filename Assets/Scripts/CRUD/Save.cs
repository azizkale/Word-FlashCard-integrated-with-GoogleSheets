using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static void saveListAllLibrariesInfo(List<AllLibrariesInfo> allLibrariesInfo)
    {
        PlayerPrefs.SetString("allLibrariesInfo", JsonConvert.SerializeObject(allLibrariesInfo));
    }

    public static void savetheLibrary(Library library)
    {
        PlayerPrefs.SetString(library.name, JsonConvert.SerializeObject(library));
    }
}
