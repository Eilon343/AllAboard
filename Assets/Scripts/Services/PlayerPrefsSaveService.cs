// PlayerPrefsSaveService.cs: today's "how", saved on the phone
using UnityEngine;
public class PlayerPrefsSaveService : ISaveService
{
    public int GetInt(string key, int fallback = 0) => PlayerPrefs.GetInt(key, fallback);
    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();                  // write now: the phone may close us any time
    }
}
