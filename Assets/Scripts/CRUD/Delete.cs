using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public static void deleteFromAllLibrariesInfo(AllLibrariesInfo singlelibraryInfo)
    {
        //calling
        List<AllLibrariesInfo> allLibrariesInfo = Read.getListAllLibrariesInfo();
        //deleting
        allLibrariesInfo.Remove(allLibrariesInfo.Find(i => i.name == singlelibraryInfo.name));
        //re-saving
        Save.saveListAllLibrariesInfo(allLibrariesInfo);
    }

    public static void deleteLibraryFromDevice(AllLibrariesInfo singlelibraryinfo)
    {
        PlayerPrefs.DeleteKey(singlelibraryinfo.name);
    }

    public static void deleteSingleWord(Word word, Library library)
    {
        library.words.Remove(library.words.Find(w => w.theWord == word.theWord && w.meaning == word.meaning));
        Save.saveSingleLibrary(library);
       
    }
}
