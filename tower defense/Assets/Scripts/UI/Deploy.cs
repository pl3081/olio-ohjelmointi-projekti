using UnityEngine;
using UnityEngine.SceneManagement;

public class Deploy : MonoBehaviour
{
    public void LoadMap()
    {
        SceneManager.LoadScene("map");
    }
}
