using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

public class UntangleManager : UntangleBase
{
    public override void Awake()
    {
        base.Awake();

        curMapData = MyGameManager.Get().selectedMapData;
        ShowMapData(curMapData);
    }

    private void Start()
    {
        //ImportVerticesData(1001);
    }

    public override void Update()
    {
        base.Update();
    }
}

