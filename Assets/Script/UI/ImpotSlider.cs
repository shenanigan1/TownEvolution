using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles updating the Impot resource value based on the slider's value.
/// </summary>
public class ImpotSlider : MonoBehaviour
{
    [SerializeField] private Slider m_slider;

    /// <summary>
    /// Adds Impot resource based on the current slider value.
    /// Assumes slider value is between 0 and 1, scaled to 0-100.
    /// </summary>
    public void SetImpotOnSlider()
    {
        int amountToAdd = Mathf.RoundToInt(m_slider.value * 100);
        RessourcesManager.Instance.AddRessources<Impot>(amountToAdd);
    }
}
