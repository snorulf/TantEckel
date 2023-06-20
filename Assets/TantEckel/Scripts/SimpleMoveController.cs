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
        transform.position += transform.forward * forwardInput * movementSpeed * Time.deltaTime;
        
        // Yaw
        float yawInput = controls.Movement.Yaw.ReadValue<float>();
        transform.Rotate(0f, yawInput * rotationSpeed * Time.deltaTime, 0f);

        // Pitch
        float pitchInput = controls.Movement.Pitch.ReadValue<float>();
        transform.Rotate(pitchInput * rotationSpeed * Time.deltaTime, 0f, 0f);

        // Altitude
        float altitudeInput = controls.Movement.Altitude.ReadValue<float>();
        transform.position += transform.up * altitudeInput * movementSpeed * Time.deltaTime;
    }
}
