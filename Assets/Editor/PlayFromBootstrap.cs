// Assets/Editor/PlayFromBootstrap.cs: runs only in the editor, never in the game
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]                      // run when Unity loads or recompiles
static class PlayFromBootstrap
{
    const string BootstrapPath = "Assets/Scenes/Bootstrap.unity";
    const string MenuPath = "Tools/Play From Bootstrap";
    const string EnabledKey = "AllAboard.PlayFromBootstrap.Enabled"; // per developer, not in git

    static bool Enabled
    {
        get => EditorPrefs.GetBool(EnabledKey, true);
        set => EditorPrefs.SetBool(EnabledKey, value);
    }

    static PlayFromBootstrap()
    {
        EditorApplication.delayCall += Apply;   // after Unity finished loading assets
        EditorApplication.playModeStateChanged += RememberOpenScene;
    }

    // Tools > Play From Bootstrap: on = every Play starts in Bootstrap, off = Play the open scene.
    [MenuItem(MenuPath)]
    static void Toggle()
    {
        Enabled = !Enabled;
        Apply();
        Debug.Log($"Play From Bootstrap: {(Enabled ? "ON" : "OFF")}");
    }

    [MenuItem(MenuPath, true)]
    static bool ToggleValidate()
    {
        Menu.SetChecked(MenuPath, Enabled);
        return !EditorApplication.isPlayingOrWillChangePlaymode;   // not while playing
    }

    static void Apply()
    {
        // Every Play starts here. When you stop, Unity reopens the scene you were editing.
        EditorSceneManager.playModeStartScene = Enabled
            ? AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapPath)
            : null;
    }

    // Right before Play: remember the scene you were editing, so Bootstrap
    // opens it instead of Home (see Bootstrap.EditorStartSceneKey).
    static void RememberOpenScene(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.ExitingEditMode) return;

        var openScene = EditorSceneManager.GetActiveScene().path;
        bool goBackToIt = Enabled && !string.IsNullOrEmpty(openScene) && openScene != BootstrapPath;
        SessionState.SetString(Bootstrap.EditorStartSceneKey, goBackToIt ? openScene : "");
    }
}
