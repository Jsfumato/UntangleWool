using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Xml.Linq;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class VecticesDataDictionary : SerializableDictionary<int, Tuple<VertexData, List<int>>> { }

public class UntangleManager : MonoBehaviour
{
    public int mapID = 1001;

    [Header("Untangle Vertex Setting")]
    public int          vertexCount;
    public VertexData   dummyVertex;
    public GameObject   dummyLine;
    public Canvas       gameCanvas;

    [Header("Export Vertex Info")]
    static int vertexId = 0;

    public VecticesDataDictionary verticesData = new VecticesDataDictionary();

    private TextAsset _verticesData;

    private void Start()
    {
        ImportVerticesData(1001);
        DrawLine();

        //SetVertex(vertexCount);

        //Observable.EveryUpdate()
        //    .Where(_ => verticesData.Count > 0)
        //    .Subscribe(_ =>
        //    {
        //        DrawLine();
        //    })
        //    .AddTo(this.gameObject);

    }

    void DrawLine()
    {
        foreach (int vDataID in verticesData.Keys) {
            VertexData vData = verticesData[vDataID].Item1;

            for (int j = 0; j < verticesData[vDataID].Item2.Count; ++j) {
                int id = verticesData[vDataID].Item2[j];

                if (verticesData.ContainsKey(id) == false)
                    continue;

                VertexData otherVData = verticesData[id].Item1;

                if(otherVData == null) {
                    verticesData.Remove(id);
                    continue;
                }

                if (verticesData[vDataID].Item2.Contains(otherVData._vertexId) == false)
                    continue;

                GameObject instLine = Instantiate(dummyLine) as GameObject;

                instLine.transform.SetParent(gameCanvas.transform, false);
                //instLine.transform.position = (vData.transform.position + otherVData.transform.position) * 0.5f;

                instLine.GetComponent<LineRenderer>().SetPositions(new Vector3[] { vData.RectTransform.position, otherVData.RectTransform.position });

                instLine.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector3.Distance(vData.transform.position, otherVData.transform.position), 16.0f);
            }
        }
    }

    public void RefreshLines()
    {

    }

    public void ImportVerticesData(int stageID)
    {
        var data = Resources.Load<TextAsset>("VerticesData");
        var doc = XDocument.Parse(data.text);

        var stageData = doc.Element("Stages");
        foreach(var stage in stageData.Elements("Stage"))
        {
            if (stage.GetAttributeLong("ID") != stageID)
                continue;

            verticesData.Clear();
            foreach (var vertex in stage.Element("Vertices").Elements("Vertex"))
            {
                // Instantiate Vertex
                VertexData newVertex = Instantiate(dummyVertex) as VertexData;
                newVertex._vertexId = (int)vertex.GetAttributeLong("ID");
                newVertex.RectTransform.SetParent(gameCanvas.transform, false);

                XElement posNode = vertex.Element("Position");
                Vector3 viewportPos = new Vector3(posNode.GetAttributeFloat("x"), posNode.GetAttributeFloat("y"), posNode.GetAttributeFloat("z"));

                Debug.Log(viewportPos);
                Vector3 worldPos = 
                    gameCanvas.worldCamera.ViewportToScreenPoint(viewportPos)
                    - new Vector3(gameCanvas.pixelRect.width * 0.5f, gameCanvas.pixelRect.height * 0.5f, 0.0f);
                Debug.Log(worldPos);
                newVertex.GetComponent<RectTransform>().localPosition = worldPos;

                newVertex.parentCanvas = gameCanvas;
                verticesData.Add(newVertex._vertexId, Tuple.Create(newVertex, new List<int>()));

                // Set Linked Vetices
                foreach (var linkedID in vertex.Element("LinkedIDs").Elements("ID"))
                    verticesData[newVertex._vertexId].Item2.Add(int.Parse(linkedID.Value));
            }
        }
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
}

// ================================================================================
#if UNITY_EDITOR
[CustomEditor(typeof(UntangleManager))]
[CanEditMultipleObjects]
public class UntangleManager_Editor : Editor
{
    UntangleManager _uManager;

    private int _mapID;
    private string _mapName;
    private string _mapDesc;

    void OnEnable()
    {
        _uManager = target as UntangleManager;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(30);

        _mapID = EditorGUILayout.IntField("mapID", _mapID);
        _mapName = EditorGUILayout.TextField("mapName", _mapName);
        _mapDesc = EditorGUILayout.TextField("mapDesc", _mapDesc);

        if (GUILayout.Button("Export Map")) {
            if (ExportMap(_mapID, _mapName, _mapDesc, _uManager.verticesData) == false)
                Debug.LogWarning("Fail to Export Data [MapID is Duplicated]");
        }
    }

    private bool ExportMap(int mapID, string mapName, string mapDesc, VecticesDataDictionary dict)
    {
        var data = Resources.Load<TextAsset>("VerticesData");
        var doc = XDocument.Parse(data.text);

        var stageData = doc.Element("Stages");
        //bool _find = false;
        foreach (var innnerStage in stageData.Elements("Stage"))
        {
            if (innnerStage.GetAttributeLong("ID") != mapID)
                continue;

            return false;
            //_find = true;
        }

        //if(_find == false)
        //{
        XElement stage = new XElement("Stage");

        stage.Add(
            new XElement("Name"),
            new XElement("Desc"),
            new XElement("Vertices")
            );

        XElement vertices = stage.Element("Vertices");
        foreach (Tuple<VertexData, List<int>> tplData in dict.Values)
        {
            Vector3 viewportPoint = _uManager.gameCanvas.worldCamera.ScreenToViewportPoint(tplData.Item1.RectTransform.position);
            Debug.Log(viewportPoint);

            var element = new XElement("Vertex",
                    new XAttribute("ID", tplData.Item1._vertexId),
                    new XElement("Position",
                        new XAttribute("x", viewportPoint.x),
                        new XAttribute("y", viewportPoint.y),
                        new XAttribute("z", viewportPoint.z)),
                    new XElement("LinkedIDs"));

            foreach (int id in tplData.Item2)
            {
                element.Element("LinkedIDs").Add(new XElement("ID", id));
            }
        }

        stageData.Add(stage);

        doc.Save("VerticesData.xml");
        return true;
    }
    
}
#endif
