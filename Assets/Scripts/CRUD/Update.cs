using Newtonsoft.Json;
using System.Collections;
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
        Save.savetheLibrary(theLibrary);

        return true;
    }

    public static void updateSingleWord(Word oldword, Library library, string newTheWord, string newMeaning)
    {
        Word newword = library.words.Find(w => w.theWord == oldword.theWord && w.meaning == oldword.meaning);

        newword.theWord = newTheWord;
        newword.meaning = newMeaning;
        newword.languageFrom = oldword.languageFrom;
        newword.languageTo = oldword.languageTo;
        newword.numberOfSight = oldword.numberOfSight;

        int index = library.words.IndexOf(oldword);

        library.words[index] = newword;
        
        //save library again
        Save.savetheLibrary(library);
    }
  
}
