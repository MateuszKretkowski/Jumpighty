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

    private void LateUpdate()
    {
        headCollider = headColliderObject.GetComponent<HeadCollider>();
        Debug.Log("isRagDolled: " + headCollider.isRagDolled);
        CameraLogic();
    }

    private void CameraLogic()
    {
        float mouseX;
        float mouseY;
        if (!headCollider.isUnRagdolledLocal)
        {
            mouseX = GetMouseInput("Mouse X");
            mouseY = GetMouseInput("Mouse Y");
        }
        else
        {
            mouseX = 0;
            mouseY = 0;
        }

        cinemachineTargetPitch = UpdateRotation(cinemachineTargetPitch, mouseY, bottomClamp, topClamp, true);
        cinemachineTargetYaw = UpdateRotation(cinemachineTargetYaw, mouseX, float.MinValue, float.MaxValue, false);

        ApplyRotations(cinemachineTargetPitch, cinemachineTargetYaw);
    }

    private void ApplyRotations(float pitch, float yaw)
    {
        followTarget.rotation = Quaternion.Euler(pitch, yaw, followTarget.eulerAngles.z);
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
