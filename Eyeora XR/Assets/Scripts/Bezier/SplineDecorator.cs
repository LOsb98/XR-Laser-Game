using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineDecorator : MonoBehaviour
{
    [SerializeField] private BezierSpline _spline;

    [SerializeField] private int _frequency;

    [SerializeField] private bool _lookForward;

    [SerializeField] private Transform[] _items;

    [SerializeField] private List<GameObject> _currentPathObjects = new List<GameObject>();

    private void Awake()
    {
        DrawNewPath();
    }

    public void ClearOldPath()
    {
        //Clearing old path

        foreach (GameObject pathObject in _currentPathObjects)
        {
            //Using DestroyImmediate since we also want to be able to call this in the editor
            DestroyImmediate(pathObject);
        }

        _currentPathObjects.Clear();
    }

    public void DrawNewPath()
    {
        ClearOldPath();

        if (_frequency <= 0 || _items == null || _items.Length == 0)
        {
            return;
        }

        float stepSize = 1f / (_frequency * _items.Length);

        for (int p = 0, f = 0; f < _frequency; f++)
        {
            for (int i = 0; i < _items.Length; i++, p++)
            {
                Transform item = Instantiate(_items[i]) as Transform;

                _currentPathObjects.Add(item.gameObject);

                Vector3 position = _spline.GetPoint(p * stepSize);

                item.transform.localPosition = position;

                if (_lookForward)
                {
                    item.transform.LookAt(position + _spline.GetDirection(p * stepSize));
                }
                item.transform.parent = transform;
            }
        }
    }
}
