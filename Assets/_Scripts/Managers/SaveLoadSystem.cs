using UnityEngine;

public static class SaveLoadSystem
{
    public enum VariablesNameTypes
    {
        NoName,
        MaxScore
    }

    public static void SaveInt(VariablesNameTypes key, int value)
    {
        PlayerPrefs.SetInt(key.ToString(), value);
    }

    public static void SaveFloat(VariablesNameTypes key, float value)
    {
        PlayerPrefs.SetFloat(key.ToString(), value);
    }

    public static void SaveString(VariablesNameTypes key, string value)
    {
        PlayerPrefs.SetString(key.ToString(), value);
    }

    public static int GetInt(VariablesNameTypes key)
    {
        return PlayerPrefs.GetInt(key.ToString());
    }

    public static float GetFloat(VariablesNameTypes key)
    {
        return PlayerPrefs.GetFloat(key.ToString());
    }

    public static string GetString(VariablesNameTypes key)
    {
        return PlayerPrefs.GetString(key.ToString());
    }
}
