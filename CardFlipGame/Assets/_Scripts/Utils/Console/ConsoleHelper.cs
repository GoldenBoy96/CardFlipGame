using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ConsoleHelper 
{
    public static void LogClearConsole(string message)
    {
        ClearConsole();
        Debug.Log(message);
    }

    public static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
