using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UntangleManager : MonoBehaviour
{
    [Header("Untangle Vertex Setting")]
    public int          vertexCount;
    public VertexData   dummyVertex;
    public GameObject   dummyLine;
    public Canvas       gameCanvas;

    [Header("Export Vertex Info")]
    static int vertexId = 0;

    private List<VertexData> _VertexList = new List<VertexData>();
    private Dictionary<Tuple<int, int>, GameObject> _lineDictionary = new Dictionary<Tuple<int, int>, GameObject>();

    private void Start()
    {
        SetVertex(vertexCount);

        Observable.EveryUpdate()
            .Where(_ => _VertexList.Count > 0)
            .Subscribe(_ =>
            {
                DrawLine();
            })
            .AddTo(this.gameObject);

    }

    void SetVertex(int vertexCount)
    {
        _VertexList.Clear();
        
        for(int i = 0; i < vertexCount; ++i)
        {
            VertexData newVertex = Instantiate(dummyVertex) as VertexData;
            newVertex._vertexId = vertexId++;
            newVertex.transform.SetParent(gameCanvas.transform, false);

            _VertexList.Add(newVertex);
        }
    }

    void DrawLine()
    {
        for(int i = 0; i < _VertexList.Count; ++i)
        {
            VertexData vData = _VertexList[i];

            for (int j = 0; j < _VertexList.Count; ++j)
            {
                VertexData otherVData = _VertexList[j];

                if (vData._linkedId.Contains(otherVData._vertexId) == false)
                    continue;

                Tuple<int, int> tpl = Tuple.Cre

                var a = Tuple<vData._vertexId, otherVData._vertexId>;
                if (_lineDictionary[Tuple.Create(vData._vertexId, otherVData._vertexId)])

                GameObject instLine = Instantiate(dummyLine) as GameObject;

                dummyLine.transform.SetParent(gameCanvas.transform, false);
                dummyLine.transform.position = (vData.transform.position + otherVData.transform.position) * 0.5f;
                dummyLine.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector3.Distance(vData.transform.position, otherVData.transform.position), 16.0f);
            }
        }
    }
}
