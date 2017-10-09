using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLineCollision : MonoBehaviour {

    //public BoxCollider lineCollider;
    //public EdgeCollider2D edgeCollider;
    //public Rigidbody rigidBody { get { return _rigidBody; } }

    private LineRenderer _lineRenderer;
    //private Rigidbody _rigidBody;

    private Material _defaultMaterial;
    public Material defaultMaterial { get { return _defaultMaterial; } }
    private bool _isCollided = false;
    public bool isCollided { get { return _isCollided; } }

    public void Awake()
    {
        _lineRenderer = transform.parent.GetComponent<LineRenderer>();
        //_rigidBody = GetComponent<Rigidbody>();

        _defaultMaterial = _lineRenderer.material;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("lineCollider"))
        {
            _lineRenderer.material = null;
            _isCollided = true;
        }
        //else
        //{
        //    _lineRenderer.material = _defaultMaterial;
        //    _isCollided = false;
        //}
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("lineCollider"))
        {
            _lineRenderer.material = _defaultMaterial;
            _isCollided = false;
        }
        //else
        //{
        //    _lineRenderer.material = null;
        //    _isCollided = true;
        //}

    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("lineCollider"))
    //        _lineRenderer.material = null;
    //    else
    //        _lineRenderer.material = _defaultMaterial;

    //}

    //public void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("lineCollider"))
    //        _lineRenderer.material = null;
    //    else
    //        _lineRenderer.material = _defaultMaterial;

    //}

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("lineCollider"))
    //        _lineRenderer.material = null;
    //    else
    //        _lineRenderer.material = _defaultMaterial;

    //}
}