using UnityEngine;
using UnityEngine.SceneManagement;

public class Area : MonoBehaviour
{
    bool _allEnemiesDead;
    [SerializeField] int reward;

    public bool EnemiesDead
    {
        get => _allEnemiesDead;
        set
        {
            _allEnemiesDead = value;
            if (!_allEnemiesDead) return;
            SceneManager.LoadScene("BuyScenario");
            Player.Instance.money += reward;
        }
    }
}
