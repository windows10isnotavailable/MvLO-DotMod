using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Audio;

using Photon.Pun;

public class GameState : Singleton<GameState>
{
    public Dictionary<string, bool> IsClearStages;

    public void Awake()
    {
        if (!InstanceCheck())
            return;

        Instance = this;

        IsClearStages = new()
        {
            ["KeepTheMushroom"] = false,
        };

        LoadGameStateFromPreferences();
    }

    public void LoadGameStateFromPreferences()
    {
        IsClearStages["KeepTheMushroom"] = PlayerPrefs.GetInt("DotMod-GS-IsClearKeepTheMushroom", 0) == 1;
    }
    public void SaveGameStateToPreferences()
    {
        PlayerPrefs.SetInt("DotMod-GS-IsClearKeepTheMushroom", IsClearStages["KeepTheMushroom"] ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsClear(string keyName)
    {
        return IsClearStages.ContainsKey(keyName) && GameState.Instance.IsClearStages[keyName];
    }
}