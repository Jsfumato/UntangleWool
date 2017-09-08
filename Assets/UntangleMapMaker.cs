using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UntangleMapMaker : MonoBehaviour
{
    public int mapID = 1001;

    [Header("Untangle Vertex Setting")]
    public int vertexCount;
    public VertexData dummyVertex;
    public GameObject dummyLine;
    public LineRenderer dummyLineRenderer;
    public Canvas gameCanvas;


    [Header("Export Vertex Info")]
    static int vertexId = 0;

    public List<MapData> mapDataList = new List<MapData>();
    public Dictionary<int, VertexData> vertexMap = new Dictionary<int, VertexData>();
    public Dictionary<Tuple<int, int>, LineRenderer> lineDict = new Dictionary<Tuple<int, int>, LineRenderer>();

    public MapData curMapData;

    public void Awake()
    {
        curMapData = new MapData();
    }

    public void Init()
    {
        mapDataList.Clear();
        vertexMap.Clear();
        lineDict.Clear();
    }

    public void Update()
    {
        foreach(var KVpair in lineDict) {
            Vector3 from = vertexMap[KVpair.Key.Item1].RectTransform.position;
            Vector3 to = vertexMap[KVpair.Key.Item2].RectTransform.position;

            curMapData.verticesData[KVpair.Key.Item1] = Tuple.Create(
                vertexMap[KVpair.Key.Item1].RectTransform.localPosition + new Vector3(gameCanvas.pixelRect.width * 0.5f, gameCanvas.pixelRect.height * 0.5f, 0.0f)
                , curMapData.verticesData[KVpair.Key.Item1].Item2);
            curMapData.verticesData[KVpair.Key.Item2] = Tuple.Create(
                vertexMap[KVpair.Key.Item2].RectTransform.localPosition + new Vector3(gameCanvas.pixelRect.width * 0.5f, gameCanvas.pixelRect.height * 0.5f, 0.0f)
                , curMapData.verticesData[KVpair.Key.Item2].Item2);

            KVpair.Value.SetPositions(new Vector3[] { from, to });
        }
    }

    public void ImportTotalMapData() {
        mapDataList.Clear();

        var data = Resources.Load<TextAsset>("VerticesData");
        var doc = XDocument.Parse(data.text);

        foreach (var stage in doc.Element("Stages").Elements("Stage"))
            mapDataList.Add(new MapData(stage));
    }

    public void ShowMapData(MapData map) {
        curMapData = map;

        //Init();

        foreach (var vKVpair in map.verticesData) {
            // Instantiate Vertex
            VertexData newVertex = Instantiate(dummyVertex) as VertexData;
            newVertex._vertexId = vKVpair.Key;
            newVertex.RectTransform.SetParent(gameCanvas.transform, false);

            Vector3 viewportPos = vKVpair.Value.Item1;
            Vector3 worldPos = gameCanvas.worldCamera.ViewportToScreenPoint(viewportPos)
                - new Vector3(gameCanvas.pixelRect.width * 0.5f, gameCanvas.pixelRect.height * 0.5f, 0.0f);
            newVertex.GetComponent<RectTransform>().localPosition = worldPos;
            newVertex.parentCanvas = gameCanvas;

            vertexMap.Add(newVertex._vertexId, newVertex);

        }

        //// Set Linked Vetices
        foreach (var vKVpair in map.verticesData) {
            foreach (var linkedID in vKVpair.Value.Item2)
                DrawLine(vKVpair.Key, linkedID);
        }
    }

    public void DrawLine(int fromID, int toID) {
        Vector3 from = vertexMap[fromID].RectTransform.position;
        Vector3 to = vertexMap[toID].RectTransform.position;

        LineRenderer newLine = Instantiate(dummyLineRenderer) as LineRenderer;
        newLine.transform.SetParent(gameCanvas.transform);
        newLine.SetPositions(new Vector3[] { from, to });

        lineDict.Add(Tuple.Create(fromID, toID), newLine);
    }

    public void AddVertex()
    {
        VertexData newVertex = Instantiate(dummyVertex) as VertexData;
        newVertex._vertexId = vertexMap.Keys.Count;
        newVertex.RectTransform.SetParent(gameCanvas.transform, false);

        Vector3 worldPos = gameCanvas.worldCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f))
            - new Vector3(gameCanvas.pixelRect.width * 0.5f, gameCanvas.pixelRect.height * 0.5f, 0.0f);
        newVertex.GetComponent<RectTransform>().localPosition = worldPos;
        newVertex.parentCanvas = gameCanvas;

        vertexMap.Add(newVertex._vertexId, newVertex);

        if (curMapData == null)
            curMapData = new MapData();
        curMapData.verticesData.Add(newVertex._vertexId, Tuple.Create(new Vector3(0.5f, 0.5f, 0.0f), new List<int>()));
    }

    public bool isSelected = false;
    public int selectedID = -1;

    public void LinkNodes(int id1, int id2)
    {
        if (id1 == id2)
            isSelected = false;

        if (curMapData.verticesData[id1].Item2.Contains(id2))
            isSelected = false;

        if (isSelected == false)
            return;

        var data1 = vertexMap[id1];
        var data2 = vertexMap[id2];

        curMapData.verticesData[id1].Item2.Add(id2);
        curMapData.verticesData[id2].Item2.Add(id1);

        Debug.LogFormat("Linked {0} and {1}", id1, id2);
        DrawLine(id1, id2);

        isSelected = false;
        selectedID = -1;
    }
}

// ================================================================================
#if UNITY_EDITOR
[CustomEditor(typeof(UntangleMapMaker))]
[CanEditMultipleObjects]
public class UntangleMapMaker_Editor : Editor
{
    UntangleMapMaker _uMapMaker;

    private int _mapID;
    private string _mapName;
    private string _mapDesc;

    void OnEnable()
    {
        _uMapMaker = target as UntangleMapMaker;

        //_uMapMaker.ImportTotalMapData();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(30);
        foreach(MapData data in _uMapMaker.mapDataList) {
            EditorGUILayout.LabelField(data.mapID.ToString());
            if (GUILayout.Button("Apply")) {
                _uMapMaker.ShowMapData(data);
            }
        }
        GUILayout.Space(30);

        _mapID = EditorGUILayout.IntField("mapID", _mapID);
        _mapName = EditorGUILayout.TextField("mapName", _mapName);
        _mapDesc = EditorGUILayout.TextField("mapDesc", _mapDesc);

        if (GUILayout.Button("Add Vertex")) {
            _uMapMaker.AddVertex();
        }
        if (GUILayout.Button("Import Map"))
        {
            _uMapMaker.ImportTotalMapData();
        }
        if (GUILayout.Button("Export Map"))
        {
            if (ExportMap(_mapID, _mapName, _mapDesc, _uMapMaker.curMapData) == false)
                Debug.LogWarning("Fail to Export Data [MapID is Duplicated]");
        }
    }

    private bool ExportMap(int mapID, string mapName, string mapDesc, MapData mData)
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

        stage.Add(new XAttribute("ID", mapID),
            new XElement("Name", mapName),
            new XElement("Desc", mapDesc),
            new XElement("Vertices")
            );

        XElement vertices = stage.Element("Vertices");
        foreach (var kvpair in mData.verticesData)
        {
            Vector3 viewportPoint = _uMapMaker.gameCanvas.worldCamera.ScreenToViewportPoint(kvpair.Value.Item1);
            Debug.Log(viewportPoint);

            var vertex = new XElement("Vertex",
                    new XAttribute("ID", kvpair.Key),
                    new XElement("Position",
                        new XAttribute("x", viewportPoint.x),
                        new XAttribute("y", viewportPoint.y),
                        new XAttribute("z", viewportPoint.z)),
                    new XElement("LinkedIDs"));

            foreach (int id in kvpair.Value.Item2)
                vertex.Element("LinkedIDs").Add(new XElement("ID", id));

            vertices.Add(vertex);
        }

        stageData.Add(stage);

        string path = string.Format("{0}/Resources/{1}.xml", Application.dataPath, "VerticesData");
        doc.Save(path);
        Debug.Log(string.Format("Saved at {0}", path));

        return true;
    }

}
#endif
