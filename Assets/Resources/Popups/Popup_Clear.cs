using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Clear : Popup_Base {

    [Header("Label")]
    public Text title;
    public Text movedCount;

    [Header("Button")]
    public Button backToMenu;
    public Button TryAgain;

    public void Initialize(MapData mapData, int count)
    {
        title.text = mapData.mapName;
        movedCount.text = string.Format("Move : {0} times", count);

        backToMenu.onClick.RemoveAllListeners();
        backToMenu.onClick.AddListener(() =>
        {
            MyGameManager.Get().LoadMenuScene();
        });

        TryAgain.onClick.RemoveAllListeners();
        TryAgain.onClick.AddListener(() =>
        {
            MyGameManager.Get().LoadGameScene(mapData);
        });
    }
}
