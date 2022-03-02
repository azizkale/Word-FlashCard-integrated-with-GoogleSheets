using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class Read : MonoBehaviour
{
    public static List<AllLibrariesInfo> getListAllLibrariesInfo()
    {
        List<AllLibrariesInfo> allLibrariesInfo = JsonConvert.DeserializeObject<List<AllLibrariesInfo>>(PlayerPrefs.GetString("allLibrariesInfo"));      

        return allLibrariesInfo;
    }

    //all libraries are stored just by their names
    public static Library getLibraryActiveWords(string libraryname)
    {
        Library theLibrary = JsonConvert.DeserializeObject<Library>(PlayerPrefs.GetString(libraryname));

        foreach (Word word in theLibrary.words.ToArray())
        {
            if (word.archive == true)
                theLibrary.words.Remove(word);
        }
       
        return theLibrary;
    }

    public static Library getLibrarysAllWords(string libraryname)
    {
        Library theLibrary = JsonConvert.DeserializeObject<Library>(PlayerPrefs.GetString(libraryname));

        return theLibrary;
    }

    public static Library getLibraryArchieveWords(string libraryname)
    {
        Library theLibrary = JsonConvert.DeserializeObject<Library>(PlayerPrefs.GetString(libraryname));

        foreach (Word word in theLibrary.words.ToArray())
        {
            if (word.archive != true)
                theLibrary.words.Remove(word);
        }

        return theLibrary;
    }

    public static Library getBeingSearchedwordsInASinglelibrary(string libraryname, string searchString)
    {
        Library theLibrary = JsonConvert.DeserializeObject<Library>(PlayerPrefs.GetString(libraryname));
        
        foreach (Word word in theLibrary.words.ToArray())
        {
            if(!(word.theWord +" "+ word.meaning).Contains(searchString))
            {
                theLibrary.words.Remove(word);
            }
        }
        return theLibrary;
    }
}
