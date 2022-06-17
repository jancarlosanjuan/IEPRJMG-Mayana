using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupUpWindow : MonoBehaviour
{
    public TriggerEvent onOpenList;
    public GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        OnHidePanel();
        onOpenList.Attach(OnShowPanel);
    }

    private void OnDisable()
    {
        onOpenList.Detach(OnShowPanel);
    }

    public void OnShowPanel()
    {
        for(int i = 0; i < objects.Length; i++)
            objects[i].SetActive(true);
        
    }

    public void OnHidePanel()
    {
        for (int i = 0; i < objects.Length; i++)
            objects[i].SetActive(false);
    }
}
