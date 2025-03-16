using UnityEngine;

public interface ICloneable<T>
{
    public T CloneSelf()
    {
        var serialized = JsonUtility.ToJson(this);
        return JsonUtility.FromJson<T>(serialized);
    }
}
