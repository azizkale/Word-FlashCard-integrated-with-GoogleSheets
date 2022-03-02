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

    //== All-Words-Library-Functions
    public static List<Word> combineAllWords()
    {
        List<Word> allWords = new List<Word>();
        List<AllLibrariesInfo> allLibrariesInfo = Read.getListAllLibrariesInfo();

        foreach (AllLibrariesInfo info in allLibrariesInfo)
        {
            Library lib = Read.getLibrarysAllWords(info.name);
            allWords.AddRange(lib.words); // all libraries are joined
        }
        return allWords;
    }

    public static List<Word> combineAllActiveWords()
    {
        List<Word> allActiveWords = new List<Word>();

        foreach (Word word in combineAllWords())
        {
            if(word.archive != false)
            {
                allActiveWords.Add(word);
            }
        }
        return allActiveWords;
    }

    public static List<Word> combineAllArchiveWords()
    {
        List<Word> allArchiveWords = new List<Word>();

        foreach (Word word in combineAllWords())
        {
            if(word.archive == true)
            {
                allArchiveWords.Add(word);
            }
        }

        return allArchiveWords;
    }
   
    public static Library getAllLibrariesWords()
    {
        Library allWordsLibrary = new Library();

        allWordsLibrary.name = "All Words";
        allWordsLibrary.language = "Multilanguage";
        allWordsLibrary.words = combineAllWords();
        allWordsLibrary.wordsCount = combineAllWords().Count;
        return allWordsLibrary;
    }

    public static Library getAllLibrariesActiveWords()
    {
        Library allActiveWordsLibrary = new Library();
        List<Word> allActiveWords = new List<Word>();

        foreach (Word word in combineAllWords())
        {
            if(word.archive == false)
            {
                allActiveWords.Add(word);
            }
        }

        allActiveWordsLibrary.name = "All Active Words";
        allActiveWordsLibrary.language = "Multilanguage";
        allActiveWordsLibrary.words = allActiveWords;
        allActiveWordsLibrary.wordsCount = allActiveWords.Count;
        return allActiveWordsLibrary;
    }

    public static Library getAllLibrariesArchiveWords()
    {
        Library allAArchiveWordsLibrary = new Library();
        List<Word> allArchiveWords = new List<Word>();

        foreach (Word word in combineAllWords())
        {
            if (word.archive == true)
            {
                allArchiveWords.Add(word);
            }
        }

        allAArchiveWordsLibrary.name = "All Archive Words";
        allAArchiveWordsLibrary.language = "Multilanguage";
        allAArchiveWordsLibrary.words = allArchiveWords;
        allAArchiveWordsLibrary.wordsCount = allArchiveWords.Count;
        return allAArchiveWordsLibrary;
    }

    public static Library getBeingSearchWordsInAlllibraries(string libraryname, string searchString)
    {
        Library theLibrary = new Library();
        theLibrary.name = libraryname;

        if(libraryname =="All Words")
        {

        }
        
        if(libraryname == "All Archive Words")
        {

        }

        foreach (Word word in theLibrary.words.ToArray())
        {
            if (!(word.theWord + " " + word.meaning).Contains(searchString))
            {
                theLibrary.words.Remove(word);
            }
        }
        return theLibrary;
    }
}
