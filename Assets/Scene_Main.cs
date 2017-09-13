using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene_Main : MonoBehaviour {

    [Header("Stage Grid")]
    public GameObject stageGrid;

    private void Awake() {
        MyGameManager.Get().Initialize();

        stageGrid.SetActive(false);
    }

    public void SetStageGrid() {
        stageGrid.SetActive(true);
    }
}