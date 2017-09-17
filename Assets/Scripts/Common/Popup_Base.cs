using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Base : MonoBehaviour {

    public virtual void Awake() {

    }

	public virtual void Start () {
		
	}

    public virtual void Update () {
		
	}

    public virtual void Hide() {
        GameObject.DestroyImmediate(this.gameObject);
    }
}
