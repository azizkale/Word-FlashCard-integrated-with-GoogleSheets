using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JsonData : MonoBehaviour
{
    private Library library;
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("http://docs.google.com/spreadsheets/d/1x2mVarvh6yopqX67QV7l-ttfblzZJLmVC40sXKdVKuE/gviz/tq?"));

        // A non-existing page.
        StartCoroutine(GetRequest("http://docs.google.com/spreadsheets/d/1x2mVarvh6yopqX67QV7l-ttfblzZJLmVC40sXKdVKuE/gviz/tq?"));

    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    {

                        string jsonString = "";
                        string str = webRequest.downloadHandler.text.Substring(47);

                        for (int i = 0; i < str.Length - 2; i++)
                        {
                            jsonString += str[i];
                        }                      

                        JObject json = JObject.Parse(jsonString);

                        JArray rows = (JArray)json["table"]["rows"];

                        library = new Library();
                        library.words = new List<Word>();
                        foreach (var item in rows)
                        {
                            Word word = new Word();
                            word.languageFrom = item["c"][0]["v"].ToString();
                            word.languageTo = item["c"][1]["v"].ToString();
                            word.theWord = item["c"][2]["v"].ToString();
                            word.meaning = item["c"][3]["v"].ToString();

                            library.words.Add(word);
                            
                        }

                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;
                    }

            }
        }
    }
}


