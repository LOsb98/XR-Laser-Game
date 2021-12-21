using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : MonoBehaviour
{
    [SerializeField] private Material _purple;
    [SerializeField] private Material _red;
    [SerializeField] private Material _green;

    [SerializeField] private MeshRenderer _mesh;

    private WalkerColourMode _colour;

    private WalkerColourMode Colour
    {
        get
        {
            return _colour;
        }
        set
        {
            _colour = value;

            switch (_colour)
            {
                case WalkerColourMode.Purple:
                    _mesh.material = _purple;
                    break;

                case WalkerColourMode.Red:
                    _mesh.material = _red;
                    break;

                case WalkerColourMode.Green:
                    _mesh.material = _green;
                    break;
            }
        }
    }

    public void InitializeRandomColour()
    {
        WalkerColourMode newColour;

        int randomIndex = Random.Range(0, 3);

        newColour = (WalkerColourMode)randomIndex;

        Colour = newColour;
    }

    public void CheckColour(WalkerColourMode hitColour)
    {
        if (hitColour == Colour)
        {
            Destroy(gameObject);

            GameManager.Instance.Score++;
        }
    }
}
