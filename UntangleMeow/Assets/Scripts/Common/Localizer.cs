using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Localizer
{
    private static Localizer _instance;

    private TextAsset localizerXml;
    private Dictionary<string, string> localizerDict = new Dictionary<string, string>();

    public Localizer Get()
    {
        if (_instance == null)
            _instance = new Localizer();

        return _instance;
    }

	void Awake()
    {
        ImportLocalizerData();
    }

    public void ImportLocalizerData()
    {
        localizerXml = Resources.Load<TextAsset>("Strings.xml");
        var doc = XDocument.Parse(localizerXml.text);

        foreach (var kvPair in doc.Element("Strings").Elements("Key"))
            localizerDict.Add(kvPair.GetAttributeString("ID"), kvPair.Value);
    }
}
