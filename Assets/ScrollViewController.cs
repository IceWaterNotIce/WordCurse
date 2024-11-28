using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class ScrollViewController : MonoBehaviour
{
    public GameObject wordItemPrefab;
    public Transform contentTransform;

    public WordJsonManager wordJsonManager;

    void Start()
    {
        Task.Delay(1000);
        LoadWords();
    }

    public void LoadWords()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        string filePath = Path.Combine(Application.streamingAssetsPath, "words.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Word[] words = JsonConvert.DeserializeObject<Word[]>(json);
            if (words == null)
            {
                return;
            }
            for (int i = 0; i < words.Length; i++)
            {
                var wordItem = Instantiate(wordItemPrefab, contentTransform).transform;
                var TmpWord = wordItem.GetChild(0);
                TmpWord.GetComponent<TextMeshProUGUI>().text = words[i].word;
                var TmpMeaning = wordItem.GetChild(1);
                TmpMeaning.GetComponent<TextMeshProUGUI>().text = words[i].definition;
                // Add a button to delete the word
                var deleteButton = wordItem.GetChild(2);
                var word = words[i].word;
                deleteButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    wordJsonManager.DeleteWord(word);
                    LoadWords();
                });
            }

            contentTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, wordItemPrefab.GetComponent<RectTransform>().rect.height * words.Length);
        }
        else
        {
            Debug.LogError("Cannot find words.json file!");
        }
    }
}