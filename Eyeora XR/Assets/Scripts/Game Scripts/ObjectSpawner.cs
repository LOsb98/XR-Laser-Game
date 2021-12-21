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

    [SerializeField] private BezierSpline _splinePath;

    [SerializeField] private GameObject _objectPrefab;

    private float _objectSpawnDelay;

    private float _objectSpawnTimer;

    private float _objectSpeed;

    public void InitializeObjectSpawner(float newObjectSpawnDelay, float newObjectSpeed)
    {
        _objectSpawnDelay = newObjectSpawnDelay;

        _objectSpawnTimer = _objectSpawnDelay;

        _objectSpeed = newObjectSpeed;
    }

    private void Update()
    {
        if (_objectSpawnTimer > 0)
        {
            _objectSpawnTimer -= Time.deltaTime;
        }
        else
        {
            //Could keep spawn delay persistent between levels (gradually decrease) OR reset between levels and decrease as more shapes are destroyed
            _objectSpawnTimer = _objectSpawnDelay;

            SpawnNewObject();
        }
    }

    private void SpawnNewObject()
    {
        GameObject newObject = Instantiate(_objectPrefab);

        //Is there a way to do this without GetComponent?
        SplineWalker newObjectScript = newObject.GetComponent<SplineWalker>();
        newObjectScript.Initialize(_splinePath, _objectSpeed);

        WalkerController newObjectData = newObject.GetComponent<WalkerController>();
        newObjectData.InitializeRandomColour();
    }

    public void ClearObjects()
    {
        //Object pooling would be worth looking at for this situation
        //Lots of the same object being instantiated and destroyed, better not to actually create and destroy them so frequently
        //Also makes getting rid of all objects on failure/success simpler
        foreach (GameObject activeWalkerObject in GameObject.FindGameObjectsWithTag("Path Object"))
        {
            Destroy(activeWalkerObject);
        }
    }
}
