using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotateCamera : MonoBehaviour
{
    [SerializeField] private Transform _parentContainer;

    [SerializeField] private float _mouseSens = 1f;
    private float _verticalRotation = 0f;

    private bool _mouseLocked;

    private bool MouseLocked
    {
        get
        {
            return _mouseLocked;
        }
        set
        {
            _mouseLocked = value;

            Cursor.lockState = _mouseLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    private void Update()
    {
        //Taking mouse input values to rotate player body
        float mouseX = Input.GetAxis("Mouse X") * _mouseSens;
        _parentContainer.Rotate(0, mouseX, 0);

        //Vertical camera rotation, clamped to 90 degrees up and down
        _verticalRotation -= Input.GetAxis("Mouse Y") * _mouseSens;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);

        if (Input.GetKeyDown(KeyCode.G))
        {
            //Toggle mouse lock state
            MouseLocked = !MouseLocked;
        }
    }
}
