using System.IO;
using UnityEngine;

public class Memory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        createTxtFile();
    }

    void createTxtFile()
    {
        string path = Application.persistentDataPath + "/words.txt";
        if (!File.Exists(path))
        {
            // creates the txt file
            File.WriteAllText(path, "");
        }
        // add line to txt file
        File.AppendAllText(path, "aiz kale \n");

        string[] words = File.ReadAllLines(path);

        foreach (string item in words)
        {
            Debug.Log(item.ToString());
        }
    }
}
