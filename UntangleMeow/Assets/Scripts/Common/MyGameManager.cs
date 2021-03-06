﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager {

    private static MyGameManager _instance;

    private RectTransform _popupParent;
    private List<Popup_Base> _popups = new List<Popup_Base>();

    public static MyGameManager Get() {
        if (_instance == null)
            _instance = new MyGameManager();
        return _instance;
    }

    private List<MapData> _mapData = new List<MapData>();
    public List<MapData> mapData { get { return _mapData; } }
    private MapData _selectedMap;
    public MapData selectedMapData { get { return _selectedMap; } }

    private int _vertexMovedCount = 0;
    public int vertexMovedCount { get { return _vertexMovedCount; } }

    public void Initialize()
    {
        _mapData.Clear();

        var data = Resources.Load<TextAsset>("VerticesData");
        var doc = XDocument.Parse(data.text);

        foreach (var stage in doc.Element("Stages").Elements("Stage"))
            _mapData.Add(new MapData(stage));

        _popupParent = GameObject.FindObjectOfType<PopupManager>().GetComponent<RectTransform>();

        HideAllPopups();
    }

    public void HideAllPopups()
    {
        if (_popupParent == null)
            return;

        for (int i = 0; i < _popups.Count; ++i)
        {
            Popup_Base pBase = _popups[i];
            if (pBase != null)
                pBase.Hide();
            _popups.Remove(pBase);
        }

        foreach (Transform go in _popupParent.transform)
            GameObject.DestroyImmediate(go.gameObject);
    }

    public T ShowPopup<T>() where T : Popup_Base
    {
        T popup = Resources.Load<T>(string.Format("Popups/{0}", typeof(T).ToString()));
        T cloned = GameObject.Instantiate(popup) as T;
        cloned.transform.SetParent(_popupParent, false);
        _popups.Add(cloned);

        return cloned;
    }

    public void HidePopup<T>() where T : Popup_Base
    {
        foreach (Transform go in _popupParent.transform)
            GameObject.DestroyImmediate(go.gameObject);
        foreach (Popup_Base pBase in _popups)
            pBase.Hide();
    }

    public void LoadGameScene(MapData mData)
    {
        _selectedMap = mData;
        _vertexMovedCount = 0;

        if (_selectedMap == null)
            return;

        SceneManager.LoadScene(2, LoadSceneMode.Single);
        HideAllPopups();
    }

    public void AddVertexMovedCount()
    {
        _vertexMovedCount++;
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        HideAllPopups();
    }

    //public void HidePopup(int instanceID)
    //{
    //    foreach (Transform go in _popupParent.transform)
    //        if (go.gameObject.GetInstanceID().Equals(instanceID))
    //            GameObject.DestroyImmediate(go.gameObject);
    //    foreach (Popup_Base pBase in _popups)
    //        pBase.Hide();
    //}
}
