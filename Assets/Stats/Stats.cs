using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Stats")]
public class Stats : ScriptableObject
{

}

public abstract class BaseStats<T>
{
    private T value = default;
    public T Value
    {
        get
        {
            return value;
        }
        set
        {
            Value = value;
        }
    }
}

[System.Serializable]
public class FloatStats : BaseStats<float>
{

}

[System.Serializable]
public class StringStats : BaseStats<string>
{

}
