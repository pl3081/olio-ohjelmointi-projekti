using UnityEngine;
using UnityEngine.SceneManagement;

public class Deploy : MonoBehaviour
{
    public void LoadMap(string map)
    {
        if (Player.Instance.TotalUnits > 0)
        {
            SceneManager.LoadScene(map);
        }
    }
}
