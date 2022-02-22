using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class Read : MonoBehaviour
{
    public static List<AllLibrariesInfo> getListAllLibrariesInfo()
    {
        List<AllLibrariesInfo> allLibrariesInfo = new List<AllLibrariesInfo>();
        allLibrariesInfo = JsonConvert.DeserializeObject<List<AllLibrariesInfo>>(PlayerPrefs.GetString("allLibrariesInfo"));

        return allLibrariesInfo;
    }

    // all libraries are stored just by their names
    public static Library getLibraryContentWithoutArchive(string libraryname)
    {
        Library theLibrary = JsonConvert.DeserializeObject<Library>(PlayerPrefs.GetString(libraryname));

        theLibrary.words.Remove(theLibrary.words.Find(w => w.archive == true));

        //foreach (Word word in theLibrary.words)
        //{
        //    if (word.archive == true)
        //        theLibrary.words.Remove(word);
        //}
        return theLibrary;
    }
}
