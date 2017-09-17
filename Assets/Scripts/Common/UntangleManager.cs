
public class UntangleManager : UntangleBase
{
    public override void Awake()
    {
        base.Awake();

        curMapData = MyGameManager.Get().selectedMapData;
        ShowMapData(curMapData);
    }

    public override void Update()
    {
        base.Update();
    }
}

