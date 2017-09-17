using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_UntangleGame : MonoBehaviour {

    public void BackToMenu()
    {
        MyGameManager.Get().LoadMenuScene();
    }
}
