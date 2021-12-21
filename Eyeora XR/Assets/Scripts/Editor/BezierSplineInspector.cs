using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//https://catlikecoding.com/unity/tutorials/curves-and-splines/

[CustomEditor (typeof (BezierSpline))]
public class BezierCurveInspector : Editor
{
    private const float _directionScale = 0.5f;

    private const int _lineSteps = 10;

    private const int _stepsPerCurve = 10;

    private const float _handleSize = 0.04f;
    private const float _pickSize = 0.06f;

    private int _selectedIndex = -1;

    private BezierSpline _spline;
    private Transform _handleTransform;
    private Quaternion _handleRotation;

    private static Color[] _modeColours =
    {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private void OnSceneGUI()
    {
        //Target = what is currently being inspected
        _spline = target as BezierSpline;

        //Drawing the line in world space instead of local space
        _handleTransform = _spline.transform;
        _handleRotation = Tools.pivotRotation == PivotRotation.Local ? _handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < _spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.red;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();

    }

    public override void OnInspectorGUI()
    {
        _spline = target as BezierSpline;

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", _spline.Loop);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_spline, "Toggle Loop");
            EditorUtility.SetDirty(_spline);
            _spline.Loop = loop;
        }

        if (_selectedIndex >= 0 && _selectedIndex < _spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }

        if (GUILayout.Button("AddCurve"))
        {
            Undo.RecordObject(_spline, "Add Curve");
            _spline.AddCurve();
            EditorUtility.SetDirty(_spline);
        }

        if (GUILayout.Button("Randomize Points"))
        {
            Undo.RecordObject(_spline, "Randomized Points");
            _spline.RandomizePoints();
            EditorUtility.SetDirty(_spline);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();

        Vector3 point = EditorGUILayout.Vector3Field("Position", _spline.GetControlPoint(_selectedIndex));

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_spline, "Move Point");
            EditorUtility.SetDirty(_spline);
            _spline.SetControlPoint(_selectedIndex, point);
        }

        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", _spline.GetControlPointMode(_selectedIndex));

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_spline, "Change Point Mode");
            _spline.SetControlPointMode(_selectedIndex, mode);
            EditorUtility.SetDirty(_spline);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = _handleTransform.TransformPoint(_spline.GetControlPoint(index));

        float size = HandleUtility.GetHandleSize(point);

        if (index == 0)
        {
            size *= 2f;
        }

        Handles.color = _modeColours[(int)_spline.GetControlPointMode(index)];
        if (Handles.Button(point, _handleRotation, size * _handleSize, size * _pickSize, Handles.DotHandleCap))
        {
            _selectedIndex = index;
            Repaint();
        }

        if (_selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();

            point = Handles.DoPositionHandle(point, _handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_spline, "Move Point");
                EditorUtility.SetDirty(_spline);
                _spline.SetControlPoint(index, _handleTransform.InverseTransformPoint(point));
            }
        }


        return point;
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = _spline.GetPoint(0f);

        Handles.DrawLine(point, point + _spline.GetDirection(0f) * _directionScale);

        int steps = _stepsPerCurve * _spline.CurveCount;

        for (int i = 1; i <= steps; i++)
        {
            point = _spline.GetPoint(i / (float)steps);

            Handles.DrawLine(point, point + _spline.GetDirection(i / (float)steps) * _directionScale);
        }
    }
}
