using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private DataController dataController;
    [SerializeField] private SoundManager soundManager;

    private List<EnglishWord> words;

    [SerializeField] private TMPro.TextMeshProUGUI TmpQuestion;
    [SerializeField] private TMP_InputField TmpInputAnswer;

    [SerializeField] private TextMeshProUGUI TmpMeaning;

    [SerializeField] private TextMeshProUGUI TmpNumTyped;

    [SerializeField] private TextMeshProUGUI TmpNumCorrect;

    [SerializeField] private TextMeshProUGUI TmpCorrectRate;

    // Start is called before the first frame update
    void Start()
    {
        words = dataController.Query();
        NewQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && TmpInputAnswer.text != "")
        {
            CheckAnswer();
        }
    }

    public void NewQuestion()
    {
        // randon get word by according the numtyped and numcorrect
        //numcorrect high, numtyped low > possibility low
        //numcorrect low, numtyped high > possibility high
        List<int> list = new List<int>();

        float meanTyped = 0;
        float meanCorrect = 0;
        foreach (var word in words)
        {
            meanTyped += word.NumTyped;
            meanCorrect += word.NumCorrect;
        }
        meanTyped /= words.Count;
        meanCorrect /= words.Count;

        Debug.Log("meanTyped : " + meanTyped);
        Debug.Log("meanCorrect : " + meanCorrect);

        TmpCorrectRate.text = "Correct Rate : " + (meanCorrect / meanTyped).ToString();

        foreach (var word in words)
        {
            for (int i = 0; i < word.NumTyped - word.NumCorrect + meanTyped - meanCorrect; i++)
            {
                list.Add(word.Id);
            }
        }

        int id = list[Random.Range(0, list.Count)];
        Debug.Log("id : " + id);
        Debug.Log("list count: " + list.Count);

        id = words.FindIndex(x => x.Id == id);
        TmpQuestion.text = words[id].Word;
        TmpQuestion.gameObject.SetActive(false);

        TmpMeaning.text = words[id].Meaning;

        AudioClip clip = Resources.Load<AudioClip>("Audios/" + TmpQuestion.text);
        soundManager.PlaySFX(clip);

        TmpInputAnswer.text = "";
        TmpInputAnswer.Select();
        TmpInputAnswer.ActivateInputField();

        TmpNumTyped.text = "NumTyped : " + words[id].NumTyped.ToString();
        TmpNumCorrect.text = "NumCorrect : " + words[id].NumCorrect.ToString();
    }

    public void CheckAnswer()
    {
        //update numtyped and numcorrect
        var word = words.Find(x => x.Word == TmpQuestion.text);
        word.NumTyped++;
        dataController.UpdateEnglishWord(word);


        if (TmpInputAnswer.text.ToLower() == TmpQuestion.text.ToLower())
        {
            //update numcorrect
            word = words.Find(x => x.Word == TmpQuestion.text);
            word.NumCorrect++;
            dataController.UpdateEnglishWord(word);
            Debug.Log("Correct");
            NewQuestion();

        }
        else
        {
            Debug.Log("Incorrect");
            ReTryQuestion(TmpQuestion.text);
        }

        
    }

    public void ReTryQuestion(string question)
    {
        TmpQuestion.text = question;

        TmpMeaning.text = words.Find(x => x.Word == question).Meaning;

        AudioClip clip = Resources.Load<AudioClip>("Audios/" + TmpQuestion.text);
        soundManager.PlaySFX(clip);

        TmpQuestion.gameObject.SetActive(true);

        TmpInputAnswer.text = "";
        TmpInputAnswer.Select();
        TmpInputAnswer.ActivateInputField();

        TmpNumTyped.text = "NumTyped : " + words.Find(x => x.Word == question).NumTyped.ToString();
        TmpNumCorrect.text = "NumCorrect : " + words.Find(x => x.Word == question).NumCorrect.ToString();
    }
}
