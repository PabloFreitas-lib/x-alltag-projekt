using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerAnimation : MonoBehaviour
{
    public XRBaseInteractable interactable; // The window with XR Grab Interactable
    public Animator windowAnimator; // The Animator of the window

    void OnEnable()
    {
        // debug log to see if is working
        interactable.onSelectEntered.AddListener(StartTurn);
        interactable.onSelectExited.AddListener(StopTurn);
    }

    void OnDisable()
    {
        // debug log to see if is working
        interactable.onSelectEntered.RemoveListener(StartTurn);
        interactable.onSelectExited.RemoveListener(StopTurn);
    }

    private void StartTurn(XRBaseInteractor interactor)
    {
        windowAnimator.SetBool("isTurning", true);
    }

    private void StopTurn(XRBaseInteractor interactor)
    {
        windowAnimator.SetBool("isTurning", false);
    }
}
