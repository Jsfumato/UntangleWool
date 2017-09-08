using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UniRx;
using UnityEngine;

[Serializable]
public class VecticesDataDictionary : SerializableDictionary<int, Tuple<VertexData, List<int>>> { }


public class MapData
{
    public int      mapID;
    public string   mapName;
    public string   mapDesc;

    //public VecticesDataDictionary verticesData = new VecticesDataDictionary();

    public Dictionary<int, Tuple<Vector3, List<int>>> verticesData = new Dictionary<int, Tuple<Vector3, List<int>>>();

    public MapData()
    {
        //mapID = (int)stage.GetAttributeLong("ID");
        //mapName = stage.Attribute("Name").Value;
        //mapDesc = stage.Attribute("Desc").Value;

        verticesData.Clear();
        //foreach (var vertex in stage.Element("Vertices").Elements("Vertex"))
        //{
        //    int vertexId = (int)vertex.GetAttributeLong("ID");

        //    XElement posNode = vertex.Element("Position");
        //    Vector3 viewportPos = new Vector3(posNode.GetAttributeFloat("x"), posNode.GetAttributeFloat("y"), posNode.GetAttributeFloat("z"));

        //    // Set Linked Vetices
        //    List<int> linkedNode = new List<int>();
        //    foreach (var linkedID in vertex.Element("LinkedIDs").Elements("ID"))
        //        linkedNode.Add(int.Parse(linkedID.Value));

        //    verticesData.Add(vertexId, Tuple.Create(viewportPos, linkedNode));
        //}
    }

    public MapData(XElement stage)
    {
        mapID = (int)stage.GetAttributeLong("ID");
        mapName = stage.Element("Name").Value;
        mapDesc = stage.Element("Desc").Value;

        verticesData.Clear();
        foreach (var vertex in stage.Element("Vertices").Elements("Vertex"))
        {
            int vertexId = (int)vertex.GetAttributeLong("ID");

            XElement posNode = vertex.Element("Position");
            Vector3 viewportPos = new Vector3(posNode.GetAttributeFloat("x"), posNode.GetAttributeFloat("y"), posNode.GetAttributeFloat("z"));
            
            // Set Linked Vetices
            List<int> linkedNode = new List<int>();
            foreach (var linkedID in vertex.Element("LinkedIDs").Elements("ID"))
                linkedNode.Add(int.Parse(linkedID.Value));

            verticesData.Add(vertexId, Tuple.Create(viewportPos, linkedNode));
        }
    }

}

public class Constants
{
    public class Map
    {
        public string Stages = "Stages";
        public string Stage = "Stage";
        public string ID = "ID";

        public string Vertices = "Vertices";
        public string Vertex = "Vertex";

        public string Position = "Position";
        public string x = "x";
        public string y = "y";
        public string z = "z";

        public string LinkedIDs = "LinkedIDs";
    }
}
