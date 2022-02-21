using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportingFuntions : MonoBehaviour
{
    public static List<Library> wordsToCreateNewLibraryOnTheDevice = new List<Library>();

   
    public static void createLanguageCards(List<GameObject> listClonePrefabLanguageCard, List<(Library lib, string libname)> librarylist, GameObject clonePrefabIportFileMenu, GameObject prefabLanguageCard)
    {
        listClonePrefabLanguageCard.Clear();

        foreach ((Library library, string libname) in librarylist)
        {
            GameObject clonePrefabLanguageCard = Instantiate(
                prefabLanguageCard,
               clonePrefabIportFileMenu.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").transform.position,
                Quaternion.identity,
                clonePrefabIportFileMenu.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").transform) as GameObject;

            clonePrefabLanguageCard.transform.Find("TMP_LanguageName").GetComponent<TextMeshProUGUI>().text = library.name;
            clonePrefabLanguageCard.transform.Find("TMP_WordCount").GetComponent<TextMeshProUGUI>().text = "Word count: " + calculateWordsCountForSingleLanguage(librarylist, library.name).ToString();

            //toggle onValueChange - gives count of words of the list has this language
            clonePrefabLanguageCard.transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener((val) => {
                displayTheWordCountOfTheNewLibraryFile(clonePrefabIportFileMenu, library.name, val,librarylist);
                //
                if (val)
                {
                    library.language = library.name;
                    wordsToCreateNewLibraryOnTheDevice.Add(library);
                }
                   
                else
                    wordsToCreateNewLibraryOnTheDevice.Remove(wordsToCreateNewLibraryOnTheDevice.Find(li => li.name == library.name));
            });
            //
            listClonePrefabLanguageCard.Add(clonePrefabLanguageCard);
        }
    }

   static int calculateWordsCountForSingleLanguage(List<(Library lib, string libname)> list, string language)
    {
        Library lib = list.Find(library => library.libname == language).lib;
        return lib.words.Count;
    }

    //gets the count of the checked libarary on the languagesInIportedFile menu on the screen
    static void displayTheWordCountOfTheNewLibraryFile(GameObject selectLanguageMenu, string libraryname, bool val, List<(Library lib, string libname)> libraryList)
    {
        int countOfSelectedLanguage = libraryList.Find(lib => lib.libname == libraryname).lib.words.Count;
        int currentCount = Int32.Parse(selectLanguageMenu.transform.Find("Header").transform.Find("WordCount").GetComponent<TextMeshProUGUI>().text);

        if (val)
        {
            selectLanguageMenu.transform.Find("Header").transform.Find("WordCount").GetComponent<TextMeshProUGUI>().text = (countOfSelectedLanguage + currentCount).ToString();
        }
        else
        {
            selectLanguageMenu.transform.Find("Header").transform.Find("WordCount").GetComponent<TextMeshProUGUI>().text = (currentCount - countOfSelectedLanguage).ToString();
        }
    }

   
}
