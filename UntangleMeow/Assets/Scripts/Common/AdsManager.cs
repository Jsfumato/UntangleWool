using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {

    private string platformGameID;

	// Use this for initialization
	void Awake() {
#if UNITY_EDITOR || UNITY_ANDROID
        platformGameID = "1557491";
#elif UNITY_IOS
        platformGameID = "1557490";
#endif
        Advertisement.Initialize(platformGameID);
    }
}
