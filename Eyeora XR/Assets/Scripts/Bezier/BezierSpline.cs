using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Spline class
/// </summary>
public class BezierSpline : MonoBehaviour
{
    //Would prefer to have these exposed in the inspector, but doesn't seem to work with this custom bezier inspector
    private const float _maxXPosition = 10f;
    private const float _maxYPosition = 4f;
    private const float _maxZPosition = 3f;

    private bool _loop;

    public bool Loop
    {
        get
        {
            return _loop;
        }
        set
        {
            _loop = value;

            if (value == true)
            {
                _controlMode[_controlMode.Length - 1] = _controlMode[0];
                SetControlPoint(0, _points[0]);
            }
        }
    }

    [SerializeField] private Vector3[] _points;

    [SerializeField] private BezierControlPointMode[] _controlMode;

    public int ControlPointCount
    {
        get
        {
            return _points.Length;
        }
    }

    public Vector3 GetControlPoint(int index)
    {
        return _points[index];
    }

    public void SetControlPoint (int index, Vector3 point)
    {
        if (index % 3 == 0)
        {
            Vector3 delta = point - _points[index];

            if (_loop)
            {
                if (index == 0)
                {
                    _points[1] += delta;
                    _points[_points.Length - 2] += delta;
                    _points[_points.Length - 1] = point;
                }
                else if (index == _points.Length - 1)
                {
                    _points[0] = point;
                    _points[1] += delta;
                    _points[index - 1] += delta;
                }
                else
                {
                    _points[index - 1] += delta;
                    _points[index + 1] = delta;
                }
            }
            else
            {
                if (index > 0)
                {
                    _points[index - 1] += delta;
                }

                if (index + 1 < _points.Length)
                {
                    _points[index + 1] += delta;
                }
            }
        }
        _points[index] = point;
        EnforceMode(index);

        SplineDecorator decorator = GetComponent<SplineDecorator>();
        decorator.DrawNewPath();
    }

    public void RandomizePoints()
    {
        //Setting each point to a random value

        //Don't do this to the first and last points
        for (int i = 1; i < _points.Length; i++)
        {
            float xPos = UnityEngine.Random.Range(1, _maxXPosition);
            float yPos = UnityEngine.Random.Range(0, _maxYPosition);
            float zPos = UnityEngine.Random.Range(-_maxZPosition + 2, _maxZPosition);

            Vector3 newPosition = new Vector3(xPos, yPos, zPos);

            if (i != _points.Length - 1)
            {
                //Set other control points
                SetControlPoint(i, newPosition);
            }
            else
            {
                //Set final control point to consistent position
                Vector3 finalPointPos = new Vector3(_maxXPosition + 2, 0, 4);
                SetControlPoint(i, finalPointPos);
            }
        }

        SplineDecorator decorator = GetComponent<SplineDecorator>();
        decorator.DrawNewPath();
    }

    public BezierControlPointMode GetControlPointMode (int index)
    {
        return _controlMode[(index + 1) / 3];
    }

    public void SetControlPointMode (int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        _controlMode[modeIndex] = mode;

        if (_loop)
        {
            if (modeIndex == 0)
            {
                _controlMode[_controlMode.Length - 1] = mode;
            }
            else if (modeIndex == _controlMode.Length - 1)
            {
                _controlMode[0] = mode;
            }
        }
        EnforceMode(index);
    }

    public void Reset()
    {
        _points = new Vector3[]
        {
            new Vector3 (1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3 (4f, 0f, 0f)
        };

        _controlMode = new BezierControlPointMode[]
        {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };

        SplineDecorator decorator = GetComponent<SplineDecorator>();
        decorator.DrawNewPath();
    }

    public int CurveCount
    {
        get
        {
            return (_points.Length - 1) / 3;
        }
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t>= 1f)
        {
            t = 1f;
            i = _points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(Bezier.GetPoint(_points[i], _points[i + 1], _points[i + 2], _points[i + 3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = _points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(Bezier.GetFirstDerivative(_points[i], _points[i + 1], _points[i + 2], _points[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void AddCurve()
    {
        Vector3 point = _points[_points.Length - 1];

        Array.Resize(ref _points, _points.Length + 3);

        point.x += 1f;
        _points[_points.Length - 3] = point;

        point.x += 1f;
        _points[_points.Length - 2] = point;

        point.x += 1f;
        _points[_points.Length - 1] = point;

        Array.Resize(ref _controlMode, _controlMode.Length + 1);
        _controlMode[_controlMode.Length - 1] = _controlMode[_controlMode.Length - 2];

        EnforceMode(_points.Length - 4);

        if (_loop)
        {
            _points[_points.Length - 1] = _points[0];
            _controlMode[_controlMode.Length - 1] = _controlMode[0];
            EnforceMode(0);
        }
    }

    private void EnforceMode (int index)
    {
        int modeIndex = (index + 1) / 3;

        BezierControlPointMode mode = _controlMode[modeIndex];

        if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == _controlMode.Length - 1)
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;

        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;

            if (fixedIndex < 0)
            {
                fixedIndex = _points.Length - 2;
            }

            enforcedIndex = middleIndex + 1;

            if (enforcedIndex >= _points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;

            if (fixedIndex >= _points.Length)
            {
                enforcedIndex = 1;
            }

            enforcedIndex = middleIndex - 1;

            if (enforcedIndex < 0)
            {
                enforcedIndex = _points.Length - 2;
            }
        }

        Vector3 middle = _points[middleIndex];
        Vector3 enforcedTangent = middle - _points[fixedIndex];

        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, _points[enforcedIndex]);
        }

        _points[enforcedIndex] = middle + enforcedTangent;
    }
}
