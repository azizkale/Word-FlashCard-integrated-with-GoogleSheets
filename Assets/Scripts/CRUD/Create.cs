using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour
{
    public static AllLibrariesInfo createAndAddNewLibraryInfo(string filename, List<Word> allwords, List<(Library lib, string libname)> selectedLibraires)
    {
        AllLibrariesInfo libinfo = new AllLibrariesInfo();
        libinfo.name = filename;
        if (selectedLibraires.Count == 1)
            libinfo.language = selectedLibraires[0].libname;
        else
            libinfo.language = "Multilingual";
        libinfo.wordsCount = allwords.Count;

        return libinfo;
    }

    public static Library createNewLibrary(string filename, List<Word> allwords, List<(Library lib, string libname)> selectedLibraires, AllLibrariesInfo libinfo)
    {
        Library lib = new Library();
        try
        {
            lib.name = filename;
            lib.wordsCount = allwords.Count;
            lib.words = allwords;
            if (selectedLibraires.Count == 1)
                lib.language = selectedLibraires[0].libname;
            else
                lib.language = "Multilingual";
            libinfo.wordsCount = allwords.Count;
            return lib;
        }
        catch (System.Exception)
        {

            throw;
        }
      
    }
}
