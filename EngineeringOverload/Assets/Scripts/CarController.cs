using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBrakeForce;
    private bool isBraking;

    // Settings
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float brakeForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;

    [SerializeField] private GameObject frontLeftWheel, frontRightWheel;
    [SerializeField] private GameObject rearLeftWheel, rearRightWheel;

    private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    private Transform frontLeftWheelTransform, frontRightWheelTransform;
    private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Awake()
    {
        AssignWheelComponents();
    }

    private void AssignWheelComponents()
    {
        AssignSingleWheel(frontLeftWheel, ref frontLeftWheelCollider, ref frontLeftWheelTransform);
        AssignSingleWheel(frontRightWheel, ref frontRightWheelCollider, ref frontRightWheelTransform);
        AssignSingleWheel(rearLeftWheel, ref rearLeftWheelCollider, ref rearLeftWheelTransform);
        AssignSingleWheel(rearRightWheel, ref rearRightWheelCollider, ref rearRightWheelTransform);
    }

    private void AssignSingleWheel(GameObject wheel, ref WheelCollider wheelCollider, ref Transform wheelTransform)
    {
        if (wheel)
        {
            wheelCollider = wheel.GetComponent<WheelCollider>();
            wheelTransform = wheel.transform;
            if (wheelCollider == null)
                Debug.LogError($"WheelCollider missing on {wheel.name}");
        }
        else
        {
            Debug.LogError("Wheel GameObject is missing in inspector!");
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        if (frontLeftWheelCollider && frontRightWheelCollider)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        ApplyBrakeToWheel(frontLeftWheelCollider);
        ApplyBrakeToWheel(frontRightWheelCollider);
        ApplyBrakeToWheel(rearLeftWheelCollider);
        ApplyBrakeToWheel(rearRightWheelCollider);
    }

    private void ApplyBrakeToWheel(WheelCollider wheelCollider)
    {
        if (wheelCollider)
            wheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;

        if (frontLeftWheelCollider) frontLeftWheelCollider.steerAngle = currentSteerAngle;
        if (frontRightWheelCollider) frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        if (wheelCollider && wheelTransform)
        {
            wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelTransform.position = pos;
            wheelTransform.rotation = rot;
        }
    }
}
