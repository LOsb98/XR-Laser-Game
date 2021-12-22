using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourButton : MonoBehaviour
{
    [SerializeField] private WalkerColourMode _buttonColour;

    public WalkerColourMode GetNewColour()
    {
        return _buttonColour;
    }
}
