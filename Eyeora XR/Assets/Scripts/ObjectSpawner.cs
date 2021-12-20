using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private int _currentStage;

    [SerializeField] private BezierSpline _splinePath;

    [SerializeField] private GameObject _objectPrefab;

    [SerializeField] private float _startingObjectSpawnDelay;

    [SerializeField] private float _startingObjectSpeed;

    private float _objectSpawnDelay;

    private float _objectSpeed;

    private void Awake()
    {
        _objectSpawnDelay = _startingObjectSpawnDelay;

        _objectSpeed = _startingObjectSpeed;
    }

    private void Update()
    {
        if (_objectSpawnDelay > 0)
        {
            _objectSpawnDelay -= Time.deltaTime;
        }
        else
        {
            //Remove this later, will not be resetting to the default spawn rate between levels (?)
            _objectSpawnDelay = _startingObjectSpawnDelay;

            GameObject newObject = Instantiate(_objectPrefab);

            SplineWalker newObjectScript = newObject.GetComponent<SplineWalker>();

            newObjectScript.Initialize(_splinePath, _objectSpeed);
        }
    }

    private void SpawnNewObject()
    {

    }

    public void StartNewStage()
    {

    }
}
