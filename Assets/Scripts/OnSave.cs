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
        mobilePath = Application.persistentDataPath;
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(mobilePath + "/" + gameManager.GoogleUser.Email + ".json", json);
    }
}
