using System;
using UnityEngine;

public abstract class Event : ScriptableObject
{
    private Action Response;

    public void Attach(Action response) => Response += response;

    public void Detach(Action response) => Response -= response;

    public void Invoke()
    {
        Response?.Invoke();
#if UNITY_EDITOR
        //Debug.Log($"{this.name} has been broadcast.");
#endif
    }
}

public abstract class Event<T> : ScriptableObject
{
    private Action<T> Response;

    public void Attach(Action<T> response)
    {
        Response += response;
    }

    public void Detach(Action<T> response)
    {
        Response -= response;
    }

    public void Invoke(T param)
    {
        Response?.Invoke(param);
#if UNITY_EDITOR
        //Debug.Log($"{this.name} has been broadcast.");
#endif
    }


}







