using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (BezierCurve))]
public class BezierCurveInspector : Editor
{
    private const int _lineSteps = 10;

    private BezierCurve _curve;
    private Transform _handleTransform;
    private Quaternion _handleRotation;

    private void OnSceneGUI()
    {
        //Target = what is currently being inspected
        _curve = target as BezierCurve;

        //Drawing the line in world space instead of local space
        _handleTransform = _curve.transform;
        _handleRotation = Tools.pivotRotation == PivotRotation.Local ? _handleTransform.rotation : Quaternion.identity;

        //Check for pivot rotation mode and adjust accordingly
        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.red;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        Handles.color = Color.white;
        Vector3 lineStart = _curve.GetPoint(0f);

        Handles.color = Color.green;
        Handles.DrawLine(lineStart, lineStart + _curve.GetDirection(0f));

        for (int i = 1; i <= _lineSteps; i++)
        {
            Vector3 lineEnd = _curve.GetPoint(i / (float)_lineSteps);
            Handles.color = Color.white;
            Handles.DrawLine(lineStart, lineEnd);
            Handles.DrawLine(lineEnd, lineEnd + _curve.GetDirection(i / (float)_lineSteps));
            lineStart = lineEnd;
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = _handleTransform.TransformPoint(_curve._points[index]);

        EditorGUI.BeginChangeCheck();

        point = Handles.DoPositionHandle(point, _handleRotation);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_curve, "Move Point");
            EditorUtility.SetDirty(_curve);
            _curve._points[index] = _handleTransform.InverseTransformPoint(point);
        }

        return point;
    }
}
