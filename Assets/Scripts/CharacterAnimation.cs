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
    private readonly float decelerationSpeedMultiplier = 3f;

    private float currentLegAngle;
    private float currentArmAngle;
    private float currentMovementTimer;
    private float currentSpeedMultiplier;

    private void Update()
    {
        if (controller.Speed > 0f)
        {
            currentMovementTimer += Time.deltaTime * movementSpeedMultiplier * controller.Speed;

            if (currentSpeedMultiplier < 1f)
            {
                currentSpeedMultiplier += Time.deltaTime * decelerationSpeedMultiplier;
                currentSpeedMultiplier = Mathf.Min(currentSpeedMultiplier, 1f);
            }
        }
        else
        {
            if (currentSpeedMultiplier > 0f)
            {
                currentSpeedMultiplier -= Time.deltaTime * decelerationSpeedMultiplier;
                currentSpeedMultiplier = Mathf.Max(currentSpeedMultiplier, 0f);
            }
        }

        currentLegAngle = Mathf.Cos(currentMovementTimer) * legMovementAngle * currentSpeedMultiplier;
        currentArmAngle = Mathf.Cos(currentMovementTimer) * armMovementAngle * currentSpeedMultiplier;

        leftLeg.localRotation = Quaternion.Euler(currentLegAngle, leftLeg.localRotation.eulerAngles.y, leftLeg.localRotation.eulerAngles.z);
        rightLeg.localRotation = Quaternion.Euler(-currentLegAngle, rightLeg.localRotation.eulerAngles.y, rightLeg.localRotation.eulerAngles.z);

        leftArm.localRotation = Quaternion.Euler(leftArm.localRotation.eulerAngles.x, leftArm.localRotation.eulerAngles.y, armRotationOffset - currentArmAngle);
        rightArm.localRotation = Quaternion.Euler(rightArm.localRotation.eulerAngles.x, rightArm.localRotation.eulerAngles.y, -armRotationOffset - currentArmAngle);
    }
}