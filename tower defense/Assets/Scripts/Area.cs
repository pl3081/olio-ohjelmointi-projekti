using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    static Area _instance;
    [SerializeField] int reward;
    [SerializeField] Behaviour failBehaviour, successBehaviour;
    
    public static List<Unit> Enemies, Units;

    static IEnumerator AreaOver(Behaviour endBehaviour)
    {
        endBehaviour.enabled = true;
        print("start waiting");
        yield return new WaitForSeconds(5);
        print("Load scene..");
        SceneManager.LoadScene("Citybase");
    }

    public static void ProcessDeath(Unit unit)
    {
        if (Enemies.Contains(unit))
        {
            Enemies.Remove(unit);
            if (Enemies.Count == 0)
            {
                _instance.StartCoroutine(AreaOver(_instance.successBehaviour));
            }
        } else if (Units.Contains(unit))
        {
            Units.Remove(unit);
            if (Units.Count == 0)
            {
                _instance.StartCoroutine(AreaOver(_instance.failBehaviour));
            }
        }
    }
    
    void Awake()
    {
        _instance = this;
        Enemies = new List<Unit>();
        Units = new List<Unit>();
    }
}
