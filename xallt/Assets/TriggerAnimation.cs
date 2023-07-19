using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
///    Hier wird grob die Aufgabe der Klasse/des Scripts beschrieben.
/// </summary>
/// <author> Autoren </author>
public class TriggerAnimation : MonoBehaviour
{
    public XRBaseInteractable interactable; // The window with XR Grab Interactable
    public Animator windowAnimator; // The Animator of the window

    /// <summary>
    ///    Hier steht ein Text der den Kontext für den ein Konstruktor gedacht ist sowie deren Eingaben     
    /// beschreibt.
    /// </summary>
    /// <author> Autoren </author>
    /// <param name="param_0"> Description </param>
    /// <param name="param_n"> Description </param>
    public TriggerAnimation(){}


    /// <summary>
    ///    Hier steht ein Text der den Kontext für den eine Funktion gedacht ist sowie deren Eingaben     
    /// beschreibt.
    /// </summary>
    /// <author> Autoren </author>
    /// <param name="param_0"> Beschreibung des Eingabewerts </param>
    /// <param name="param_n"> Beschreibung des Eingabewerts </param>
    /// <returns> Beschreibung des Rückgabewerts </return>
    void OnEnable()
    {
        // debug log to see if is working
        interactable.onSelectEntered.AddListener(StartTurn);
        interactable.onSelectExited.AddListener(StopTurn);
    }

    /// <summary>
    ///    Hier steht ein Text der den Kontext für den eine Funktion gedacht ist sowie deren Eingaben
    /// beschreibt.
    /// </summary>
    /// <author> Autoren </author>
    /// <param name="param_0"> Beschreibung des Eingabewerts </param>
    /// <param name="param_n"> Beschreibung des Eingabewerts </param>
    /// <returns> Beschreibung des Rückgabewerts </return>
    void OnDisable()
    {
        // debug log to see if is working
        interactable.onSelectEntered.RemoveListener(StartTurn);
        interactable.onSelectExited.RemoveListener(StopTurn);
    }
    /// <summary>
    ///    Hier steht ein Text der den Kontext für den eine Funktion gedacht ist sowie deren Eingaben
    /// beschreibt.
    /// </summary>
    /// <author> Autoren </author>
    /// <param name="param_0"> Beschreibung des Eingabewerts </param>
    /// <param name="param_n"> Beschreibung des Eingabewerts </param>
    /// <returns> Beschreibung des Rückgabewerts </return>
    private void StartTurn(XRBaseInteractor interactor)
    {
        windowAnimator.SetBool("isTurning", true);
    }

    /// <summary>
    ///    Hier steht ein Text der den Kontext für den eine Funktion gedacht ist sowie deren Eingaben
    /// beschreibt.
    /// </summary>
    /// <author> Autoren </author>
    /// <param name="param_0"> Beschreibung des Eingabewerts </param>
    /// <param name="param_n"> Beschreibung des Eingabewerts </param>
    /// <returns> Beschreibung des Rückgabewerts </return>
    private void StopTurn(XRBaseInteractor interactor)
    {
        windowAnimator.SetBool("isTurning", false);
    }
}
