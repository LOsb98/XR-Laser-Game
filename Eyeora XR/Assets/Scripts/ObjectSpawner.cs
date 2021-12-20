using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for spawning objects which will follow the set path
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
    /*
     * Possible difficulty ideas:
     * - Use current level as a multiplier to change stats (object spawn rate/move speed)
     * - Spawn multiple tracks for player to keep track of (maybe best as a separate mode?)
     * - Player has to match object colour AND shape to destroy them (two sets of buttons?)
     */
    [SerializeField] private int _currentStage;

    [SerializeField] private BezierSpline _splinePath;

    [SerializeField] private GameObject _objectPrefab;

    [SerializeField] private float _startingObjectSpawnDelay;

    [SerializeField] private float _startingObjectSpeed;

    private float _objectSpawnDelay;

    private float _objectSpeed;

    private void Awake()
    {
        ResetObjectSpawner();
    }

    private void Update()
    {
        if (_objectSpawnDelay > 0)
        {
            _objectSpawnDelay -= Time.deltaTime;
        }
        else
        {
            //Could keep spawn delay persistent between levels (gradually decrease) OR reset between levels and decrease as more shapes are destroyed
            _objectSpawnDelay = _startingObjectSpawnDelay;

            SpawnNewObject();
        }
    }

    private void SpawnNewObject()
    {
        GameObject newObject = Instantiate(_objectPrefab);

        //Is there a way to do this without GetComponent?
        SplineWalker newObjectScript = newObject.GetComponent<SplineWalker>();

        newObjectScript.Initialize(_splinePath, _objectSpeed);
    }

    /// <summary>
    /// Reset object spawner delay and spawned object speed
    /// </summary>
    private void ResetObjectSpawner()
    {
        //Might need to make this method public to call from a game manager?

        _objectSpawnDelay = _startingObjectSpawnDelay;

        _objectSpeed = _startingObjectSpeed;
    }
}
