using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private FirstPersonController controller;
    [SerializeField] private Transform leftLeg;
    [SerializeField] private Transform rightLeg;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioSource audioSource;

    private readonly float legMovementAngle = 20f;
    private readonly float armMovementAngle = 60f;
    private readonly float armRotationOffset = 70f;
    private readonly float movementSpeedMultiplier = 2f;
    private readonly float decelerationSpeedMultiplier = 3f;
    private readonly float armGestureSpeedMultiplier = 0.8f;
    private readonly float armGestureAngle = 75f;

    private float currentLegAngle;
    private float currentLeftArmAngle;
    private float currentRightArmAngle;
    private float currentMovementTimer;
    private float currentSpeedMultiplier;

    private bool isShowingLeftArmGesture;
    private bool isShowingRightArmGesture;
    private float currentLeftArmGestureTimer;
    private float currentRightArmGestureTimer;

    private void Update()
    {
        UpdateInput();
        UpdateAnimations();
    }

    private void UpdateInput()
    {
        var keyboardInput = Keyboard.current;

        if (keyboardInput != null && keyboardInput.lKey.wasPressedThisFrame)
        {
            ToggleLeftArmGesture(!isShowingLeftArmGesture);
        }

        if (keyboardInput != null && keyboardInput.rKey.wasPressedThisFrame)
        {
            ToggleRightArmGesture(!isShowingRightArmGesture);
        }
    }

    private void UpdateAnimations()
    {
        if (controller.Speed > 0f)
        {
            StepSound(controller.Speed);
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

        if (!isShowingLeftArmGesture)
        {
            currentLeftArmAngle = Mathf.Cos(currentMovementTimer) * armMovementAngle * currentSpeedMultiplier;
        }
        else
        {
            if (currentLeftArmGestureTimer < 1f)
            {
                currentLeftArmGestureTimer += Time.deltaTime * armGestureSpeedMultiplier;
                currentLeftArmGestureTimer = Mathf.Min(currentLeftArmGestureTimer, 1f);
            }

            currentLeftArmAngle = Mathf.Lerp(currentLeftArmAngle, armGestureAngle, currentLeftArmGestureTimer);
        }

        if (!isShowingRightArmGesture)
        {
            currentRightArmAngle = Mathf.Cos(currentMovementTimer) * armMovementAngle * currentSpeedMultiplier;
        }
        else
        {
            if (currentRightArmGestureTimer < 1f)
            {
                currentRightArmGestureTimer += Time.deltaTime * armGestureSpeedMultiplier;
                currentRightArmGestureTimer = Mathf.Min(currentRightArmGestureTimer, 1f);
            }

            currentRightArmAngle = Mathf.Lerp(currentRightArmAngle, -armGestureAngle, currentRightArmGestureTimer);
        }

        leftLeg.localRotation = Quaternion.Euler(currentLegAngle, leftLeg.localRotation.eulerAngles.y, leftLeg.localRotation.eulerAngles.z);
        rightLeg.localRotation = Quaternion.Euler(-currentLegAngle, rightLeg.localRotation.eulerAngles.y, rightLeg.localRotation.eulerAngles.z);
        leftArm.localRotation = Quaternion.Euler(leftArm.localRotation.eulerAngles.x, leftArm.localRotation.eulerAngles.y, armRotationOffset - currentLeftArmAngle);
        rightArm.localRotation = Quaternion.Euler(rightArm.localRotation.eulerAngles.x, rightArm.localRotation.eulerAngles.y, -armRotationOffset - currentRightArmAngle);
    }
    
    private void StepSound(float movementSpeed)
    {
        if (audioSource == null)
        {
            return;
        }
        if (audioSource.isPlaying)
        {
            return;
        }

        int random = Random.Range(0, stepSounds.Length);
        audioSource.PlayOneShot(stepSounds[random], Mathf.Clamp01(movementSpeed));
    }

    public void ToggleLeftArmGesture(bool toggle)
    {
        isShowingLeftArmGesture = toggle;

        if (!toggle)
        {
            currentLeftArmGestureTimer = 0f;
        }
    }

    public void ToggleRightArmGesture(bool toggle)
    {
        isShowingRightArmGesture = toggle;

        if (!toggle)
        {
            currentRightArmGestureTimer = 0f;
        }
    }
}