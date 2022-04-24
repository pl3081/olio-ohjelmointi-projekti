using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    static LevelManager _instance = null;
    public static LevelManager Instance {
        get
        {
            if (_instance == null)
            {
                _instance = new LevelManager();
            }
            return _instance;
        }
    }

    public static List<string> Levels = new List<string>()
    {
        "Game"
    };
    public static void LoadLevel(int i)
    {
        LoadLevel(Levels[i]);
    }
    public static void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }



    List<int> completedLevels = new List<int>();
    public List<int> Completed => new List<int>(completedLevels);

    public bool IsCompleted(int i)
    {
        return Completed.Contains(i);
    }
    public bool IsCompleted(string name)
    {
        return IsCompleted(Levels.FindIndex(a => a.Contains(name)));
    }
    
    public void Complete(int i)
    {
        if (!IsCompleted(i))
        {
            completedLevels.Add(i);
        }
    }
    public void Complete(string name)
    {
        Complete(Levels.FindIndex(a => a.Contains(name)));
    }

    public void LoadCompleted(List<int> completed)
    {
        completedLevels = new List<int>(completed);
    }
}
