using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] float clampAngle = 80f;
    [SerializeField] float horizontalSpeed = 10f;
    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] Transform playerTransform;

    Vector3 startingRot;
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRot == null)
                    startingRot = transform.localRotation.eulerAngles;
                Vector2 deltaInput = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                startingRot.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRot.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRot.y = Mathf.Clamp(startingRot.y, -clampAngle, clampAngle);

                state.RawOrientation = Quaternion.Euler(-startingRot.y, startingRot.x, 0f);
                playerTransform.localRotation = Quaternion.Euler(0f, startingRot.x, 0f);
            }
        }
    }
}
