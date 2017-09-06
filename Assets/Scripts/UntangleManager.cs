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

    //void SetVertex(int vertexCount)
    //{
    //    verticesData.Clear();
        
    //    for(int i = 0; i < vertexCount; ++i)
    //    {
    //        VertexData newVertex = Instantiate(dummyVertex) as VertexData;
    //        newVertex._vertexId = vertexId++;
    //        newVertex.transform.SetParent(gameCanvas.transform, false);
    //        newVertex.parentCanvas = gameCanvas;

    //        verticesData.Add(newVertex._vertexId, Tuple.Create(newVertex, new List<int>()));
    //    }
    //}

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
        //string text = Utility <string>("VerticesData.xml");
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
                Vector3 pos = new Vector3(posNode.GetAttributeFloat("x"), posNode.GetAttributeFloat("y"), posNode.GetAttributeFloat("z"));
                newVertex.GetComponent<RectTransform>().position = pos;
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
    //public static long GetAttributeLong(this XElement element, string AttrName, long defaultValue = long.MinValue)
    //{
    //    if (element.Attribute(AttrName) == null)
    //        return defaultValue;

    //    long ret;
    //    if (long.TryParse(element.Attribute(AttrName).Value, out ret) == false)
    //        return defaultValue;

    //    return ret;
    //}


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

    void OnEnable()
    {
        _uManager = target as UntangleManager;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

    }

    private void AddQuest()
    {
        //XElement qElement = new XElement("Quest", new XAttribute("ID", -1),
        //    new XElement("Name", ""),
        //    new XElement("Desc", ""),
        //    new XElement("NPC", 100),
        //    new XElement("Conversation",
        //        new XElement("Start", new XAttribute("Count", 0)),
        //        new XElement("Accept", new XAttribute("Count", 0)),
        //        new XElement("Deny", new XAttribute("Count", 0)),
        //        new XElement("Complete", new XAttribute("Count", 0))),
        //    new XElement("Condition", new XAttribute("Count", 0)),
        //    new XElement("Objective", new XAttribute("Type", 0),
        //        new XElement("ID", 0),
        //        new XElement("Count", 0)),
        //    new XElement("Rewards", new XAttribute("Count", 0)));

    }

    private void RemoveQuest(int questID) 
    {
        //if (_qManager._resQuestList.Where(x => (int)x.id == questID).ToList().Count <= 0)
        //    Debug.LogError("There is no Quest");

        //ResourceQuest resQ = _qManager._resQuestList.Find(x => (int)x.id == questID);
        //_qManager._resQuestList.Remove(resQ);
    }


    private void ExportTutorialXmlFile(string fileName)
    {
        //XElement doc = new XElement("Quests");

        //foreach (var quest in _qManager._resQuestList)
        //{
        //    XElement qElement = new XElement("Quest", new XAttribute("ID", quest.id),
        //        new XElement("Name", quest.name),
        //        new XElement("Desc", quest.desc),
        //        new XElement("NPC", quest.npcId),
        //        new XElement("Conversation",
        //            new XElement("Start", new XAttribute("Count", quest.startConversation.Count)),
        //            new XElement("Accept", new XAttribute("Count", quest.acceptConversation.Count)),
        //            new XElement("Deny", new XAttribute("Count", quest.denyConversation.Count)),
        //            new XElement("Complete", new XAttribute("Count", quest.completeConversation.Count))),
        //        new XElement("Condition", new XAttribute("Count", quest.conditionList.Count)),
        //        new XElement("Objective", new XAttribute("Type", quest.objective.Value1)),
        //        new XElement("Rewards", new XAttribute("Count", quest.rewards.Count))
        //        );

        //    // Add texts in conversation
        //    foreach (var tplLongString in quest.startConversation)
        //        qElement.Descendants("Start").First().Add(new XElement("Text", tplLongString.Value2, new XAttribute("Speaker", tplLongString.Value1)));
        //    foreach (var tplLongString in quest.acceptConversation)
        //        qElement.Descendants("Accept").First().Add(new XElement("Text", tplLongString.Value2, new XAttribute("Speaker", tplLongString.Value1)));
        //    foreach (var tplLongString in quest.denyConversation)
        //        qElement.Descendants("Deny").First().Add(new XElement("Text", tplLongString.Value2, new XAttribute("Speaker", tplLongString.Value1)));
        //    foreach (var tplLongString in quest.completeConversation)
        //        qElement.Descendants("Complete").First().Add(new XElement("Text", tplLongString.Value2, new XAttribute("Speaker", tplLongString.Value1)));

        //    // Set Condition
        //    var cElement = qElement.Descendants("Condition").First();
        //    foreach (var subconditionList in quest.conditionList)
        //    {
        //        var element = new XElement("Subcondition");
        //        for (int i = 0; i < subconditionList.Count; ++i)
        //        {
        //            var conType = subconditionList[i].param1;
        //            element.Add(new XElement("Param",
        //                new XAttribute("Type", conType),
        //                new XAttribute("Param1", subconditionList[i].param2),
        //                new XAttribute("Param2", subconditionList[i].param3)));
        //        }
        //        cElement.Add(element);
        //    }

        //    // Set Objective
        //    if (quest.objective.Value1 == ResourceQuest.ObjectiveType.MOVE_TO_MAP)
        //        qElement.Descendants("Objective").First().Add(new XElement("ID", quest.objective.Value2), new XElement("IsForced", quest.objective.Value3 != 0 ? bool.TrueString : bool.FalseString));
        //    else
        //        qElement.Descendants("Objective").First().Add(new XElement("ID", quest.objective.Value2), new XElement("Count", quest.objective.Value3));
        //    qElement.Descendants("Objective").First().SetAttributeValue("Type", quest.objective.Value1);

        //    // Add Rewards
        //    for (int i = 0; i < quest.rewards.Keys.Count; ++i)
        //    {
        //        var rewardID = quest.rewards.Keys[i];
        //        var rewardParam = quest.rewards.Values[i];

        //        qElement.Descendants("Rewards").First().Add(
        //            new XElement("Reward", rewardParam, new XAttribute("ID", rewardID)));
        //    }

        //    doc.Add(qElement);
        //}

        //string path = string.Format("{0}/PatchResources/{1}.xml", Application.dataPath, fileName);
        //doc.Save(path);

        //Debug.Log(string.Format("Saved at {0}", path));
    }
}
#endif
