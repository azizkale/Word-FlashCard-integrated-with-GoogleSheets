using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static void saveListAllLibrariesInfo(List<AllLibrariesInfo> allLibrariesInfo)
    {
        PlayerPrefs.SetString("allLibrariesInfo", JsonConvert.SerializeObject(allLibrariesInfo));
    }

    public static void saveSingleLibrary(Library library)
    {
        PlayerPrefs.SetString(library.name, JsonConvert.SerializeObject(library));

        List<AllLibrariesInfo> allLibrariesInfo = Read.getListAllLibrariesInfo();

        AllLibrariesInfo libinfo = allLibrariesInfo.Find(libi => libi.name == library.name);

        libinfo.name = library.name;
        libinfo.wordsCount = library.words.Count;
        libinfo.language = library.language;

        saveListAllLibrariesInfo(allLibrariesInfo);
    }
}
