using System.Collections;
using System.Collections.Generic;

public class MyGameManager {

    private static MyGameManager _instance;

    public static MyGameManager Get() {
        if (_instance == null)
            _instance = new MyGameManager();
        return _instance;
    }

    private List<MapData> _mapData = new List<MapData>();

    public void Initialize()
    {

    }
}
