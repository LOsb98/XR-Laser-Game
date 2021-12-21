using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class InitializeXR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VRStart());
    }

    public IEnumerator VRStart()
    {
        var xrManager = XRGeneralSettings.Instance.Manager;
        if (!xrManager.isInitializationComplete)
        {
            yield return xrManager.InitializeLoader();
            xrManager.StartSubsystems();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

        }

    }
}
