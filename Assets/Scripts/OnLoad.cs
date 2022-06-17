using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class OnLoad : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public ScriptableGameOBJ playerData;
    public string mobilePath;

    public TriggerEvent onLoad;

    private void OnEnable()
    {
        onLoad.Attach(LoadJSON);
    }

    private void OnDisable()
    {
        onLoad.Detach(LoadJSON);
    }

    public void LoadJSON()
    {
        mobilePath = Application.persistentDataPath;
        string json = File.ReadAllText(mobilePath + "/" + gameManager.GoogleUser.Email + ".json");
        JsonUtility.FromJsonOverwrite(json, playerData);
    }
}
