using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene_UntangleGame : MonoBehaviour {

    [Header("UntangleGameScene")]
    public Text countText;

    public void BackToMenu()
    {
        MyGameManager.Get().LoadMenuScene();
    }

    public void Update()
    {
        countText.text = string.Format("Count : {0}", MyGameManager.Get().vertexMovedCount);
    }
}
