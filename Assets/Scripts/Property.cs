using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property<T> : MonoBehaviour where T : Component
{
    private T property;

    public T Value
    {
        get
        {
            if(!property)
            {
                property = GetComponent<T>();
            }
            return property;
        }
    }
}
