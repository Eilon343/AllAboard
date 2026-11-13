using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFlow : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level");
    }
    public void Home()
    {
        SceneManager.LoadScene("Home");
    }
}

