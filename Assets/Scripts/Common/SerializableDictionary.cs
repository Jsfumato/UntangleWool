using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Unity can't serialize Dictionaries, so this works around it by using Lists instead.
// Details here: http://answers.unity3d.com/questions/460727/how-to-serialize-dictionary-with-unity-serializati.html
// NOTE: The TKey and TValue types you pass in MUST be seriliable themselves. If they aren't
//       try using the generic derived class technique found at the same link.

[Serializable]
public class SerializableDictionary<TKey, TValue>
    : SortedDictionary<TKey, TValue>, ISerializationCallbackReceiver/*, IEnumerable*/
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public new bool Add(TKey key, TValue value)
    {
        if (keys.Contains(key))
            return false;

        keys.Add(key);
        values.Add(value);

        return true;
    }

    public new bool Remove(TKey key)
    {
        if (!keys.Contains(key))
            return false;

        int index = keys.IndexOf(key);
        keys.RemoveAt(index);
        values.RemoveAt(index);

        return true;
    }

    public new bool RemoveAt(int index)
    {
        if (index > keys.Count && index > values.Count)
            return false;

        keys.RemoveAt(index);
        values.RemoveAt(index);

        return true;
    }

    public new bool Clear()
    {
        keys.Clear();
        values.Clear();

        return true;
    }

    public new bool ContainsKey(TKey key)
    {
        if (keys.Contains(key))
            return true;

        return false;
    }

    public new bool ContainsValue(TValue value)
    {
        if (values.Contains(value))
            return true;

        return false;
    }

    public new List<TKey> Keys { get { return keys; } }
    public new List<TValue> Values { get { return values; } }

    public new int Count { get { return keys.Count; } }

    public new TValue this[TKey key]
    {
        get
        {
            int index = keys.IndexOf(key);
            return values[index];
        }
        set
        {
            int index = keys.IndexOf(key);
            values[index] = value;
        }
    }

    public new bool TryGetValue(TKey key, out TValue value)
    {
        if (Keys.Contains(key) == false)
        {
            value = default(TValue);
            return false;
        }

        value = this[key];
        return true;
    }


    //public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    //{
    //    for(int i = 0; i < Keys.Count; ++i)
    //    { 
    //        yield return new KeyValuePair<TKey, TValue>(Keys[i], Values[i]);
    //    }

    //    yield break;
    //}

    //IEnumerator IEnumerable.GetEnumerator()
    //{
    //    return GetEnumerator();
    //}

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        //		keys.Clear();
        //		values.Clear();
        //		foreach(KeyValuePair<TKey, TValue> pair in this)
        //		{
        //			keys.Add(pair.Key);
        //			values.Add(pair.Value);
        //		}
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < keys.Count; i++)
            this[keys[i]] = i < values.Count ? values[i] : default(TValue);
    }
}