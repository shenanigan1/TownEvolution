using UnityEngine;
using UnityEngine.UI;

public class ImpotSlider : MonoBehaviour
{
    [SerializeField] private Slider m_slider;

    public void SetImpotOnSlider()
    {
        RessourcesManager.Instance.AddRessources<Impot>((int)(m_slider.value*100));
    }
}
