using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Area : MonoBehaviour
{
    bool _areaCleared;
    [SerializeField] int reward;
    public static List<Unit> Enemies { get; } = new List<Unit>();
    public static List<Unit> Units { get;  } = new List<Unit>();
    
    public bool AreaCleared
    {
        get => _areaCleared;
        set
        {
            _areaCleared = value;
            if (!_areaCleared) return;
            SceneManager.LoadScene("BuyScenario");
            Player.Instance.money += reward;
        }
    }
}
