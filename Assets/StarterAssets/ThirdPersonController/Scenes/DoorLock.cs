using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public bool isLocked = true; // Initially, the door is locked
    private Animator doorAnimator; // For door opening animation

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    public void UnlockDoor()
    {
        isLocked = false;
        if (doorAnimator)
        {
            doorAnimator.SetTrigger("Open"); // Play door opening animation
        }
        else
        {
            transform.Rotate(0, 90, 0); // Simple rotation if no animation
        }
    }
}
