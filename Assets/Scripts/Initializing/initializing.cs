using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class initializing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public static void initializingtheapp()
    {

        //as initializing it was created a string array to store libraries in it
      
        List<AllLibrariesInfo> allLibrariesInfo = new List<AllLibrariesInfo>();
        Save.saveListAllLibrariesInfo(allLibrariesInfo);
        AllLibrariesInfo libInfo = new AllLibrariesInfo();
        List<Word> wordsList = new List<Word>();
        Library lib = new Library();      

        string[] alllines = {
        "German\tEnglish\tWenn du Besuch empfangen kannst, werde ich der erste in der Reihe sein, die Schlange steht.\tIf you can have visitors, I'll be the first in line.",
            "German\tEnglish\tSie hat es mich gebeten, dich zu grüßen.\tShe asked me to greet you.",
            "German\tEnglish\tIn diesem Moment hatte ich großes Mitleid mit ihm.\tAt that moment I felt very sorry for him.",
            "German\tEnglish\tDie Familie hatte ein schlimmes Ereignis durchgemacht.\tThe family had gone through a bad event.",
            "German\tEnglish\tDas heißt...\tThat means..."
        };

        foreach (string line in alllines)
        {
            string[] str = line.Split('\t');
            Word word = new Word();
            word.theWord = str[2];
            word.meaning = str[3];
            word.languageFrom = str[0];
            word.languageTo = str[1];
            word.libraryName = "Sample Library";
            wordsList.Add(word);
        }           


        //creating library
        lib.name = "Sample Library";
        lib.language = "German-English";
        lib.wordsCount = wordsList.Count;
        lib.words = wordsList;

        PlayerPrefs.SetString(lib.name,JsonConvert.SerializeObject(lib));
        //Save.saveSingleLibrary(lib);

        //creating info about the sample library
        libInfo.name = "Sample Library";
        libInfo.language = "German-English";
        libInfo.wordsCount = lib.wordsCount;

        allLibrariesInfo.Add(libInfo);
        PlayerPrefs.SetString("allLibrariesInfo", JsonConvert.SerializeObject(allLibrariesInfo));
        //Save.saveListAllLibrariesInfo(allLibrariesInfo);       
    }
}
