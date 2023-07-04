using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class SimpleMoveController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;

    private SimpleMoveControls controls;

    private void Awake()
    {
        controls = new SimpleMoveControls();
    }

    public void OnEnable()
    {
        controls.Enable();
    }

    public void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        // Movement in forward direction
        float forwardInput = controls.Movement.Forward.ReadValue<float>();
        transform.localPosition += transform.forward * forwardInput * movementSpeed * Time.deltaTime;

        // Yaw and Pitch
        float yawInput = controls.Movement.Yaw.ReadValue<float>();
        float pitchInput = controls.Movement.Pitch.ReadValue<float>();
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x /* + (pitchInput * rotationSpeed * Time.deltaTime)*/, transform.localRotation.eulerAngles.y + (yawInput * rotationSpeed * Time.deltaTime), 0f);


        // Altitude
        float altitudeInput = controls.Movement.Altitude.ReadValue<float>();
        transform.localPosition += transform.up * altitudeInput * movementSpeed * Time.deltaTime;
    }
}
