using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    [SerializeField] private SplineWalkerMode _mode;

    [SerializeField] private BezierSpline _spline;

    [SerializeField] private float _duration;

    private float _progress;

    private bool _goingForward = true;

    public void Initialize(BezierSpline newSpline, float durationToCompleteTrack)
    {
        _spline = newSpline;

        _duration = durationToCompleteTrack;
    }

    private void Update()
    {
        if (_goingForward)
        {
            _progress += Time.deltaTime / _duration;

            if (_progress > 1f)
            {
                if (_mode == SplineWalkerMode.Once)
                {
                    _progress = 1f;
                }
                else if (_mode == SplineWalkerMode.Loop)
                {
                    _progress -= 1f;
                }
                else
                {
                    _progress = 2f - _progress;
                    _goingForward = true;
                }
            }
        }
        else
        {
            _progress -= Time.deltaTime / _duration;

            if (_progress < 0f)
            {
                _progress = -_progress;
                _goingForward = true;
            }
        }

        Vector3 newPosition = _spline.GetPoint(_progress);

        transform.localPosition = newPosition;
    }
}
