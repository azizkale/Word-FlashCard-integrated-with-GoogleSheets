using System.Collections;
using System.Collections.Generic;

public static class CommonVariables 
{
    //public static List<Word> allData = new List<Word>();
    //public static ArrayList allFileTyps = new ArrayList();
    public static string libraryName; // to open libarary content scene
    //public static List<AllLibrariesInfo> allLibrariesInfo = new List<AllLibrariesInfo>(); // it is used at loading saved libraies from device
    //    
    public static Library selectedLibraryContetnt = new Library();

    public static string charachterLimit(string text, int limit)
    {
        string newText = "";
        if (text.Length > limit)
        {
            for (int i = 0; i <= limit-2; i++)
            {
                newText += text[i];
            }
            newText = newText + "...";

        }
        else
            newText = text;

            return newText;
    }


}
