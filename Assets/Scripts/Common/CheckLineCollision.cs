using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLineCollision : MonoBehaviour {

    //public BoxCollider lineCollider;
    public EdgeCollider2D edgeCollider;
    private LineRenderer _lineRenderer;

    private Material _defaultMaterial;

    public void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _defaultMaterial = _lineRenderer.material;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("lineCollider"))
            _lineRenderer.material = null;
        else
            _lineRenderer.material = _defaultMaterial;

    }
}