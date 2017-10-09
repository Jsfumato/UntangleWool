using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

//public class Utility : MonoBehaviour {

	
//}

public static class XElementExtensions
{
    public static XmlNode ToXmlNode(this XElement element)
    {
        if (element == null)
            return null;

        XmlNode node = null;
        using (var reader = element.CreateReader())
        {
            var xml = new XmlDocument();
            xml.Load(reader);

            node = xml.DocumentElement;
        }
        return node;
    }
}

public static class ResourceExtensions
{
    public static long GetAttributeLong(this XElement element, string AttrName, long defaultValue = long.MinValue)
    {
        if (element.Attribute(AttrName) == null)
            return defaultValue;

        long ret;
        if (long.TryParse(element.Attribute(AttrName).Value, out ret) == false)
            return defaultValue;

        return ret;
    }

    public static float GetAttributeFloat(this XElement element, string AttrName, float defaultValue = float.NaN)
    {
        if (element.Attribute(AttrName) == null)
            return defaultValue;

        float ret;
        if (float.TryParse(element.Attribute(AttrName).Value, out ret) == false)
            return defaultValue;

        return ret;
    }

    public static string GetAttributeString(this XElement element, string AttrName, string defaultValue = "")
    {
        if (element.Attribute(AttrName) == null)
            return defaultValue;

        return element.Attribute(AttrName).Value;
    }
}
