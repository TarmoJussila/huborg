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
    private readonly float armMovementAngle = 20f;
    private readonly float movementSpeedMultiplier = 5f;

    private float timer;

    private void Start()
    {

    }

    private void Update()
    {
        if (controller.Speed > 0f)
        {
            timer += Time.deltaTime * movementSpeedMultiplier * controller.Speed;

            var legAngle = Mathf.Cos(timer) * legMovementAngle;
            var armAngle = Mathf.Cos(timer) * armMovementAngle;

            leftLeg.localRotation = Quaternion.Euler(legAngle, leftLeg.localRotation.y, leftLeg.localRotation.z);
            rightLeg.localRotation = Quaternion.Euler(-legAngle, rightLeg.localRotation.y, rightLeg.localRotation.z);

            leftArm.localRotation = Quaternion.Euler(armAngle, leftArm.localRotation.y, leftArm.localRotation.z);
            rightArm.localRotation = Quaternion.Euler(-armAngle, rightArm.localRotation.y, rightArm.localRotation.z);
        }
    }
}