using Newtonsoft.Json;
using UnityEngine;

public interface ICloneable<T>
{
    public T CloneSelf()
    {
        var serialized = JsonConvert.SerializeObject(this, Formatting.Indented); ;
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}
