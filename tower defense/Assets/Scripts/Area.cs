using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    [SerializeField] int reward;
    [SerializeField] Text failText, successText;
    static Text FailText, SuccessText;
    public static List<Unit> Enemies, Units;
    public static int Reward;

    public static void ProcessDeath(Unit unit)
    {
        if (Enemies.Contains(unit))
        {
            Enemies.Remove(unit);
            if (Enemies.Count == 0)
            {
                SuccessText.enabled = true;
                Player.Instance.money += Reward;
                SceneManager.LoadScene("Citybase");
            }
        } else if (Units.Contains(unit))
        {
            Units.Remove(unit);
            if (Units.Count == 0)
            {
                FailText.enabled = true;
                SceneManager.LoadScene("Citybase");
            }
        }
    }
    
    void Awake()
    {
        Reward = reward;
        Enemies = new List<Unit>();
        Units = new List<Unit>();
        FailText = failText;
        SuccessText = successText;
    }
}
