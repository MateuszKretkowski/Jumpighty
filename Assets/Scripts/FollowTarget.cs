using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    [SerializeField] private Transform followTarget;

    [SerializeField] private float rotationalSpeed = 10f;
    [SerializeField] private float bottomClamp = -40f;
    [SerializeField] private float topClamp = 70f;

    private float cinemachineTargetPitch;
    private float cinemachineTargetYaw;

    public GameObject headColliderObject;
    HeadCollider headCollider;

    public GameObject rotationObject;
    public PlayerController controller;

    void Start()
    {
        cinemachineTargetPitch = 0f;
        cinemachineTargetYaw = 0f;
    }

    private void LateUpdate()
    {
        headCollider = headColliderObject.GetComponent<HeadCollider>();
        Debug.Log("isRagDolled: " + headCollider.isRagDolled);
        CameraLogic();
    }
        public float mouseX = 0;
        public float mouseY = 0;

    private void CameraLogic()
    {

            mouseX = GetMouseInput("Mouse X");
            mouseY = GetMouseInput("Mouse Y");

        cinemachineTargetPitch = UpdateRotation(cinemachineTargetPitch, mouseY, bottomClamp, topClamp, true);
        cinemachineTargetYaw = UpdateRotation(cinemachineTargetYaw, mouseX, float.MinValue, float.MaxValue, false);

        ApplyRotations(cinemachineTargetPitch, cinemachineTargetYaw);
    }

    private void ApplyRotations(float pitch, float yaw)
    {
        followTarget.rotation = Quaternion.Euler(pitch, yaw, followTarget.eulerAngles.z);

        if (!controller.hasRotated)
        {
            rotationObject.transform.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }

    private float UpdateRotation(float currentRotation, float input, float min, float max, bool isXAxis)
    {
        currentRotation += isXAxis ? -input : input;
        return Mathf.Clamp(currentRotation, min, max);
    }

    private float GetMouseInput(string axis)
    {
        return Input.GetAxis(axis) * rotationalSpeed * Time.deltaTime;
    }
}
