using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFireRaycast : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private LayerMask _layersToHit;

    [SerializeField] private Color _purple;
    [SerializeField] private Color _red;
    [SerializeField] private Color _green;

    [SerializeField] private Image _crosshairImage;

    private WalkerColourMode _laserColour;

    public  WalkerColourMode LaserColour
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
        //Replaced with mouse input for editor testing, normally check TouchCount/TouchPhase
        if (Input.GetMouseButtonDown(0))
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        RaycastHit laserRay;

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out laserRay, 9999, _layersToHit))
        {
            Transform hitObject = laserRay.transform;

            if (hitObject.TryGetComponent<WalkerController>(out WalkerController controller))
            {
                controller.CheckColour(LaserColour);
            }
            else if (hitObject.TryGetComponent<ColourButton>(out ColourButton button))
            {
                LaserColour = button.GetNewColour();
            }
        }
    }


}
