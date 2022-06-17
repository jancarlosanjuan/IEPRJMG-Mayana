using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupUpWindow : MonoBehaviour
{
    [SerializeField]
    private TriggerEvent onOpenList;
    [SerializeField]
    private TriggerEvent onCloseList;
    [SerializeField]
    private GameObject[] objects;
    
    // Start is called before the first frame update
    void Awake()
    {
        OnHidePanel();
        onOpenList.Attach(OnShowPanel);
        onCloseList.Attach(OnHidePanel);
    }

    private void OnDisable()
    {
        onOpenList.Detach(OnShowPanel);
        onCloseList.Detach(OnHidePanel);
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
