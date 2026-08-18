using UnityEngine;

public interface IGameStorage
{
    bool HasKey(string key);
    int GetInt(string key, int defaultValue = 0);
    void SetInt(string key, int value);
    float GetFloat(string key, float defaultValue = 0f);
    void SetFloat(string key, float value);
    void Save();
}

public class PlayerPrefsStorage : IGameStorage
{
    public bool HasKey(string key) => PlayerPrefs.HasKey(key);

    public int GetInt(string key, int value = 0) => PlayerPrefs.GetInt(key, value);

    public void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);

    public float GetFloat(string key, float value = 0f) => PlayerPrefs.GetFloat(key, value);

    public void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);

    public void Save() => PlayerPrefs.Save();
}
