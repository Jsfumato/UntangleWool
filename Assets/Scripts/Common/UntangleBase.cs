using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UniRx;
using UnityEngine;

public class UntangleBase : MonoBehaviour {

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

    private int _moveCount = 0;
    public int moveCount { get { return _moveCount; } }

    public virtual void Awake()
    {
        Init();
    }

    public virtual void Update()
    {
        foreach (var KVpair in lineDict)
        {

            curMapData.verticesData[KVpair.Key.Item1] = Tuple.Create(
                vertexMap[KVpair.Key.Item1].RectTransform.localPosition
                , curMapData.verticesData[KVpair.Key.Item1].Item2);
            curMapData.verticesData[KVpair.Key.Item2] = Tuple.Create(
                vertexMap[KVpair.Key.Item2].RectTransform.localPosition
                , curMapData.verticesData[KVpair.Key.Item2].Item2);

            Vector3 from = curMapData.verticesData[KVpair.Key.Item1].Item1;
            Vector3 to = curMapData.verticesData[KVpair.Key.Item2].Item1;
            
            KVpair.Value.SetPositions(new Vector3[] {
                new Vector3(from.x, from.y, -100.0f),
                new Vector3(to.x, to.y, -100.0f)
            });

            UpdateLineCollider(KVpair.Value);
        }
    }

    public void Init()
    {
        curMapData = new MapData();

        mapDataList.Clear();
        vertexMap.Clear();
        lineDict.Clear();

        _moveCount = 0;
    }

    public void ShowMapData(MapData map)
    {
        curMapData = map;

        //Init();

        foreach (var vKVpair in map.verticesData)
        {
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
        foreach (var vKVpair in map.verticesData)
        {
            foreach (var linkedID in vKVpair.Value.Item2)
                DrawLine(vKVpair.Key, linkedID);
        }
    }

    public void DrawLine(int fromID, int toID)
    {
        Vector3 from = vertexMap[fromID].RectTransform.position;
        Vector3 to = vertexMap[toID].RectTransform.position;

        LineRenderer newLine = Instantiate(dummyLineRenderer) as LineRenderer;
        newLine.transform.SetParent(gameCanvas.transform, false);
        newLine.transform.localPosition = Vector3.zero;

        //CapsuleCollider capsule = newLine.gameObject.AddComponent<CapsuleCollider>();
        //capsule.radius = newLine.startWidth / 2;
        //capsule.center = Vector3.zero;
        //capsule.direction = 1;

        newLine.SetPositions(new Vector3[] { from, to });
        newLine.useWorldSpace = false;

        lineDict.Add(Tuple.Create(fromID, toID), newLine);



        // Init line collider
        BoxCollider lineCollider = new GameObject("LineCollider").AddComponent<BoxCollider>();
        lineCollider.transform.SetParent(newLine.transform, false);
        float lineWidth = newLine.endWidth;
        float lineLength = Vector3.Distance(newLine.GetPosition(0), newLine.GetPosition(1));
        // size of collider is set where X is length of line, Y is width of line
        //z will be how far the collider reaches to the sky
        lineCollider.size = new Vector3(lineLength, lineWidth, 1f);
        // get the midPoint
        Vector3 midPoint = (newLine.GetPosition(0) + newLine.GetPosition(1)) / 2;
        lineCollider.transform.position = midPoint;

        float angle = Mathf.Atan2(
            (newLine.GetPosition(1).y - newLine.GetPosition(0).y),
            (newLine.GetPosition(1).x - newLine.GetPosition(0).x));

        // angle now holds our answer but it's in radians, we want degrees
        // Mathf.Rad2Deg is just a constant equal to 57.2958 that we multiply by to change radians to degrees
        angle *= Mathf.Rad2Deg;

        //were interested in the inverse so multiply by -1
        //angle *= -1;
        // now apply the rotation to the collider's transform, carful where you put the angle variable
        // in 3d space you don't wan't to rotate on your y axis
        lineCollider.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void UpdateLineCollider(LineRenderer newLine)
    {
        BoxCollider lineCollider = newLine.GetComponentInChildren<BoxCollider>();
        float lineWidth = newLine.endWidth;
        float lineLength = Vector3.Distance(newLine.GetPosition(0), newLine.GetPosition(1));
        lineCollider.size = new Vector3(lineLength, lineWidth, 1f);
        Vector3 midPoint = (newLine.GetPosition(0) + newLine.GetPosition(1)) / 2;
        lineCollider.transform.localPosition = midPoint;

        float angle = Mathf.Atan2(
            (newLine.GetPosition(1).y - newLine.GetPosition(0).y),
            (newLine.GetPosition(1).x - newLine.GetPosition(0).x));
        angle *= Mathf.Rad2Deg;

        //angle *= -1;
        lineCollider.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
}



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
}
