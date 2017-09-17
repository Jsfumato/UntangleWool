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

        SetGridData(MyGameManager.Get().mapData);
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

    public void SetGridData(List<MapData> mdataList)
    {
        cell.SetActive(false);
        for(int i = 1; i < stageContent.transform.childCount; ++i) {
            Destroy(stageContent.transform.GetChild(i).gameObject);
        }

        foreach (var mdata in mdataList) {
            GameObject cloned = Instantiate(cell) as GameObject;
            cloned.SetActive(true);
            cloned.transform.SetParent(stageContent.transform, false);

            cloned.GetComponent<Button>().onClick.RemoveAllListeners();
            cloned.GetComponent<Button>().onClick.AddListener(() =>
            {
                MyGameManager.Get().ShowPopup<Popup_MapPreview>().Initialize(mdata);
            });
        }
    }
}