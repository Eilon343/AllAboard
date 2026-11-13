using UnityEngine;
using UnityEngine.SceneManagement;

// The only script in the Bootstrap scene, the first scene in the Scene List.
public class Bootstrap : MonoBehaviour
{
    // Editor only: the scene you were editing when you pressed Play (set by Assets/Editor/PlayFromBootstrap.cs).
    public const string EditorStartSceneKey = "AllAboard.PlayFromBootstrap.StartScene";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);       // Services survives every scene change

        // Start every service here, in order
        Services.Save = new PlayerPrefsSaveService();   // 1 · save

        Debug.Log("Bootstrap: services ready");

#if UNITY_EDITOR
        // Editor only: go straight to the scene you pressed Play from. On the phone this block doesn't exist.
        var editorScene = UnityEditor.SessionState.GetString(EditorStartSceneKey, "");
        if (!string.IsNullOrEmpty(editorScene))
        {
            UnityEditor.SessionState.EraseString(EditorStartSceneKey);
            UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(
                editorScene, new LoadSceneParameters(LoadSceneMode.Single));
            return;
        }
#endif

        SceneManager.LoadScene("Home");      // only now: the first screen
    }
}
