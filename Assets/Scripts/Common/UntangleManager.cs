
using UniRx;
using UnityEngine;

public class UntangleManager : UntangleBase
{
    private bool isClear = false;

    public override void Awake()
    {
        base.Awake();

        isClear = false;
        curMapData = MyGameManager.Get().selectedMapData;
        ShowMapData(curMapData);
    }

    public override void Update()
    {
        base.Update();

        if (isClear)
            return;

        bool collided = false;
        foreach (var KVpair in lineDict)
        {
            CheckLineCollision checkCol = KVpair.Value.GetComponentInChildren<CheckLineCollision>();
            collided = collided && checkCol.isCollided;
        }

        if (collided == false && MyGameManager.Get().vertexMovedCount > 0)
        {
            isClear = true;
            MyGameManager.Get().ShowPopup<Popup_Clear>().Initialize(curMapData, MyGameManager.Get().vertexMovedCount);
        }
    }
}

