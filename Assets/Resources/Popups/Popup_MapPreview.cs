using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_MapPreview : Popup_Base
{
    public Text title;
    public Image mapPreview;

    private MapData _mapData;

    public void Initialize(MapData mData)
    {
        title.text = mData.mapName;

        _mapData = mData;
    }

    public void LoadMapDate()
    {
        MyGameManager.Get().LoadGameScene(_mapData);
        //SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
