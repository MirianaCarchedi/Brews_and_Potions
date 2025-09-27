using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    private void OnEnable()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        //  Aggiorna il valore del cursore in base al volume attuale
        if (AudioManager.Instance != null)
            slider.value = AudioManager.Instance.musicVolume;

        //  Aggiungi listener per cambiare il volume
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }
}
