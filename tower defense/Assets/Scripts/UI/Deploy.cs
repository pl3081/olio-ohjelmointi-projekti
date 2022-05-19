using UnityEngine;
using UnityEngine.SceneManagement;

public class Deploy : MonoBehaviour
{
    public void LoadMap(string map)
    {
        SceneManager.LoadScene(map);
    }
}
