using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class OnSave : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    public ScriptableGameOBJ playerData;
    public string mobilePath;

    public TriggerEvent onSave;

    private void OnEnable()
    {
        onSave.Attach(SaveJSON);
    }

    private void OnDisable()
    {
        onSave.Detach(SaveJSON);
    }

    public void SaveJSON()
    {
        mobilePath = System.IO.Directory.GetCurrentDirectory();
        Debug.Log("Data Path: "  + mobilePath);


        //string json = JsonUtility.ToJson(playerData);
        //string json = "testing";

        //File.WriteAllText(mobilePath + "/" + "testjson" + ".json", json);
    }
}
