using System.Collections.Generic;
using UnityEngine;

public class Update : MonoBehaviour
{

    public static bool addingThelibraryToExistOne(Library theLibrary, List<Library> librariesToAdd)
    {
        //adds the libraries in "librariesToAdd" to the "theLibrary"
        foreach (Library item in librariesToAdd)
        {
            theLibrary.words.AddRange(item.words);
        }
        theLibrary.wordsCount = theLibrary.words.Count;
    

        //the library-info is updated        
        List<AllLibrariesInfo> allLibrariesInfo = Read.getListAllLibrariesInfo();
        AllLibrariesInfo theupdatedLibraryInfo = allLibrariesInfo.Find(i => i.name == theLibrary.name);

        theupdatedLibraryInfo.wordsCount = theLibrary.words.Count;


        foreach (Library item in librariesToAdd)
        {
            if (theLibrary.language != item.language)
            {
                theupdatedLibraryInfo.language = "Multilanguage";
                theLibrary.language = "Multilanguage";
            }
            //!!!if not the language remains the same!!!
        }

        //the updated allLibrariesInfo is stored
        Save.saveListAllLibrariesInfo(allLibrariesInfo);
        //the updated library is stored
        Save.saveSingleLibrary(theLibrary);

        return true;
    }

    public static void updateSingleWord(Word oldword, Library library, string newTheWord, string newMeaning)
    {
        Word newword = library.words.Find(w => w.theWord == oldword.theWord && w.meaning == oldword.meaning);

        newword.theWord = newTheWord;
        newword.meaning = newMeaning;
        newword.languageFrom = oldword.languageFrom;
        newword.languageTo = oldword.languageTo;
        newword.viewCount = oldword.viewCount;

        int index = library.words.IndexOf(oldword);

        library.words[index] = newword;
        
        //save library again
        Save.saveSingleLibrary(library);
    }

    public static void renameLibrary(AllLibrariesInfo info, string newname)
    {
        //calls the the list allLibrariesInfo
        List<AllLibrariesInfo> allLibrariesInfo = Read.getListAllLibrariesInfo();

        //calls the library from the device by its old name
        Library callinglab = Read.getLibraryActiveWords(info.name);
               
        //creating new Info for the being updated library
        AllLibrariesInfo newInfo = new AllLibrariesInfo();
        newInfo.name = newname;
        newInfo.wordsCount = callinglab.words.Count;
        newInfo.language = callinglab.language;

        // deleting old info from allLibrariesInfo
        allLibrariesInfo.Remove(allLibrariesInfo.Find(i => i.name == info.name));

        //adding new info
        allLibrariesInfo.Add(newInfo);

        //re-saved the allLibrariesInfo in the device
        Save.saveListAllLibrariesInfo(allLibrariesInfo);

        //delete the the library from device at first
        PlayerPrefs.DeleteKey(info.name);

        //and re-saved the library by its new name (newname)
        callinglab.name = newname;
        Save.saveSingleLibrary(callinglab);
    }
   
    public static void updateSingleLibrary(Library oldlibrary, Library newlibrary)
    {

    }
}
