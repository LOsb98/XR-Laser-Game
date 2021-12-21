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

    /// <summary>
    /// Used to set up a newly spawned object so it follows the correct path, and decide its speed
    /// </summary>
    /// <param name="newSpline">The spline for the object to follow</param>
    /// <param name="durationToCompleteTrack">How long it takes the object to reach the end of the path</param>
    public void Initialize(BezierSpline newSpline, float durationToCompleteTrack)
    {
        _spline = newSpline;

        _duration = durationToCompleteTrack;
    }

    private void Update()
    {
        //Object currently reaches the end of the path in a set time, but the path can potentially vary in length
        //Might introduce inconsistency, possible way to measure line length and adjust accordingly?

        if (_goingForward)
        {
            _progress += Time.deltaTime / _duration;

            if (_progress > 1f)
            {
                if (_mode == SplineWalkerMode.Once)
                {
                    //Want the walker object to take care of its own destruction
                    //Want to keep each one as decoupled as possible
                    GameManager.Instance.RemainingLives--;
                    Debug.Log("Object reached end of path");
                    Destroy(gameObject);
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
