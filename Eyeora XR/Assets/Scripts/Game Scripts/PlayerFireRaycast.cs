using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFireRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask _layersToHit;

    [SerializeField] private Color _purple;
    [SerializeField] private Color _red;
    [SerializeField] private Color _green;

    [SerializeField] private Image _crosshairImage;
    
    private WalkerColourMode _laserColour
    {
        get
        {
            return _laserColour;
        }
        set
        {
            _laserColour = value;

            //This switch is essentially repeated in WalkerController, could be optimised?
            //Scriptable object could hold colour information/materials?

            switch (_laserColour)
            {
                case WalkerColourMode.Purple:
                    _crosshairImage.color = _purple;
                    break;

                case WalkerColourMode.Red:
                    _crosshairImage.color = _red;
                    break;

                case WalkerColourMode.Green:
                    _crosshairImage.color = _green;
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch newTouch = Input.GetTouch(0);

            if (newTouch.phase == TouchPhase.Began)
            {
                FireLaser();
            }
        }
    }

    private void FireLaser()
    {
        RaycastHit laserRay;

        if (Physics.Raycast(transform.position, transform.forward, out laserRay, 9999, _layersToHit))
        {
            Transform hitObject = laserRay.transform;

            hitObject.GetComponent<WalkerController>().CheckColour(_laserColour);
        }
    }

    public void SetLaserColour(int laserColourIndex)
    {
        WalkerColourMode newColour;

        newColour = (WalkerColourMode)laserColourIndex;

        _laserColour = newColour;
    }
}
