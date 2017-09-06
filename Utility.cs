using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Utility
{
    public static T LoadResource<T>(string path)
    {
        object obj = null;
        Type type = typeof(T);

        Type archivedType = type == typeof(byte[]) || type == typeof(string) ? typeof(TextAsset) : type;

        //if (obj == null)
        //    obj = AssetBundleManager.Get().GetAsset(Path.GetFileName(path), archivedType);

        obj = Resources.Load<TextAsset>(path);

        //
        if (obj == null)
        {
            Debug.LogWarning("Utility::LoadResource() Failed to load " + path);
        }
        else
        {
            if (obj.GetType() == typeof(TextAsset))
            {
                if (type == typeof(byte[]))
                {
                    return (T)(object)((TextAsset)obj).bytes;
                }
                else if (type == typeof(string))
                {
                    return (T)(object)((TextAsset)obj).text.Replace("\ufeff", "");
                }
                else
                {
                    Debug.LogWarning("Utility::LoadResource() Not supported TextAsset. path=" + path);
                }
            }
        }

        return (T)(object)obj;
    }
}

