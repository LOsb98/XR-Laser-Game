using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPathEndPoint : MonoBehaviour
{
    [SerializeField] private BezierSpline _spline;

    [SerializeField] private Transform _endPointObject;

    private void Awake()
    {
        SetPathEndPointPosition();
    }

    private Vector3 FindLastPathPoint()
    {
        int lastPointIndex = _spline.ControlPointCount - 1;

        Vector3 lastPointPosition = _spline.GetPoint(lastPointIndex);

        return lastPointPosition;
    }

    private void SetPathEndPointPosition()
    {
        _endPointObject.position = FindLastPathPoint();
    }
}
