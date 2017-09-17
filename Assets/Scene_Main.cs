using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Xml.Linq;

public class Scene_Main : MonoBehaviour {

    [Header("Stage Grid")]
    public GameObject stageGrid;
    public GridLayoutGroup stageContent;
    public GameObject cell;

    private bool stageGridOpen = false;
    private List<MapData> _mapDataList = new List<MapData>();

    private void Awake() {
        MyGameManager.Get().Initialize();
        InitMapData();

        SetGridData();
    }

    public void ToggleStageGrid() {
        stageGrid.transform.DOKill();
        stageGrid.transform.DOLocalMoveY((stageGridOpen == true ? 0 : 1080) - 640.0f, 0.5f);
        stageGridOpen = !stageGridOpen;
        //stageGrid.SetActive(!stageGrid.activeSelf);
    }

    public void SetStageGrid() {
        stageGrid.SetActive(true);
    }

    private void InitMapData()
    {
        var data = Resources.Load<TextAsset>("VerticesData");
        var doc = XDocument.Parse(data.text);

        foreach (var stage in doc.Element("Stages").Elements("Stage"))
            _mapDataList.Add(new MapData(stage));
    }

    public void SetGridData()
    {
        cell.SetActive(false);
        for(int i = 1; i < stageContent.transform.childCount; ++i) {
            Destroy(stageContent.transform.GetChild(i).gameObject);
        }

        foreach (var mdata in _mapDataList) {
            GameObject cloned = Instantiate(cell) as GameObject;
            cloned.SetActive(true);
            cloned.transform.SetParent(stageContent.transform, false);
        }
    }
}