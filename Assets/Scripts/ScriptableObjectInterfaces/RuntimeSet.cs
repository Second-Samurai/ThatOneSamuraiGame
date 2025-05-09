using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    private List<T> items = new List<T>();

    public void Add(T t)
    {
        if(!items.Contains(t)) items.Add(t);
    }
    
    public void Remove(T t)
    {
        if(items.Contains(t)) items.Remove(t);
    }

    public T GetItemIndex(int index)
    {
        return items[index];
    }
    
    public void PrintItemList()
    {
        if (items.Count > 0)
        {
            for(int i = items.Count - 1; i >= 0; i--)
            {
                Debug.Log("Item " + i + ": " + items[i]);
            }
        }
        else
        {
            Debug.Log("No items in this list");
        }
    }

    public void Clear()
    {
        items.Clear();
    }

    public int Count()
    {
        return items.Count;
    }
}
