using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public TMP_InputField nameP;
    public string currentPlayer;
    public string bestPlayer;
    public int bestScore;
    public TMP_Text bestScore_txt;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CheckSaveFile();
    }

    [System.Serializable]
    class SaveScore
    {
        public string Name;
        public int score;
    }

    public void CheckSaveFile()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveScore data = JsonUtility.FromJson<SaveScore>(json);

            bestPlayer = data.Name;
            bestScore = data.score;

            bestScore_txt.text = "Best Score: " + data.Name + " " + data.score;
        }
    }
    public void CreateSave()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        SaveScore data = new SaveScore();
        data.Name = nameP.text;
        data.score = 0;

        currentPlayer = data.Name;
        bestPlayer = data.Name;
        bestScore = data.score;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void UpdateIt(int points)
    {
        if(points > bestScore)
        {
            bestPlayer = currentPlayer;
            bestScore = points;

            string path = Application.persistentDataPath + "/savedata.json";
            SaveScore data = new SaveScore();
            data.Name = bestPlayer;
            data.score = bestScore;

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
    }

    public void StartGame()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (!File.Exists(path))
        {
            CreateSave();
        }
        currentPlayer = nameP.text;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
