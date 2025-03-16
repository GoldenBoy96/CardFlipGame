using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObserverHelper : MonoBehaviour
{
    static Dictionary<string, List<Action<object[]>>> Listeners =
        new();

    public static void RegisterListener(string name, Action<object[]> callback)
    {
        Debug.Log(callback);
        if (!Listeners.ContainsKey(name))
        {
            Listeners.Add(name, new List<Action<object[]>>());
        }

        if (!Listeners[name].Contains(callback)) {
            Listeners[name].Add(callback);
        }
    }

    public static void RemoveListener(string name, Action<object[]> callback)
    {
        if (!Listeners.ContainsKey(name))
        {
            return;
        }

        Listeners[name].Remove(callback);
    }

    public static void Notify(string name, params object[] data)
    {
        Debug.Log(data);
        if (!Listeners.ContainsKey(name))
        {
            return;
        }

        foreach (var listener in Listeners[name].ToList())
        {
            //listener.Invoke(data);
            try
            {
                listener.Invoke(data);
            }
            catch (Exception ex)
            {
                RemoveListener(name, listener);
                //Debug.LogError("Error on invoke listener: " + ex);
                //TO DO: Remove only exceptions created by destroying GameObject
            }
        }
    }

}
