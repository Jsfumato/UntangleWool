using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Localizer
{
    private static Localizer _instance;

    private TextAsset localizerXml;

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
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ImportLocalizerData()
    {
        localizerXml = Resources.Load<TextAsset>("Localizer");
        var doc = XDocument.Parse(localizerXml.text);

        foreach (var stage in doc.Element("Stages").Elements("Stage"))
            mapDataList.Add(new MapData(stage));
    }
}
