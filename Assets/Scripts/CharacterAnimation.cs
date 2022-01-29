using StarterAssets;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private FirstPersonController controller;

    [SerializeField] private Transform leftLeg;
    [SerializeField] private Transform rightLeg;

    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;

    private readonly float legMovementAngle = 20f;
    private readonly float armMovementAngle = 60f;
    private readonly float armRotationOffset = 70f;
    private readonly float movementSpeedMultiplier = 2f;

    private float timer;

    private void Update()
    {
        if (controller.Speed > 0f)
        {
            timer += Time.deltaTime * movementSpeedMultiplier * controller.Speed;

            var legAngle = Mathf.Cos(timer) * legMovementAngle;
            var armAngle = Mathf.Cos(timer) * armMovementAngle;

            leftLeg.localRotation = Quaternion.Euler(legAngle, leftLeg.localRotation.eulerAngles.y, leftLeg.localRotation.eulerAngles.z);
            rightLeg.localRotation = Quaternion.Euler(-legAngle, rightLeg.localRotation.eulerAngles.y, rightLeg.localRotation.eulerAngles.z);

            leftArm.localRotation = Quaternion.Euler(leftArm.localRotation.eulerAngles.x, leftArm.localRotation.eulerAngles.y, armRotationOffset - armAngle);
            rightArm.localRotation = Quaternion.Euler(rightArm.localRotation.eulerAngles.x, rightArm.localRotation.eulerAngles.y, -armRotationOffset - armAngle);
        }
    }
}