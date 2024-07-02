using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public AudioMixer mixer;
    public AudioMixerGroup group;
    public Slider slider;
    public TextMeshProUGUI sliderTitle;
    public AudioSource sampleAudioSource;

    // Start is called before the first frame update
    public void Start()
    {
        float val;

        sliderTitle.text = group.name;
        mixer.GetFloat(group.name, out val);
        slider.value = ConvertDbToRange(val);
    }

    public void PlayTestSound()
    {
        Debug.Log(sliderTitle.text);
        Util.AttemptPlay(sampleAudioSource);
    }
    public void SetVolume(float valueIncoming)
    {
        mixer.SetFloat(group.name, Mathf.Log10(Mathf.Clamp(valueIncoming, 0.0001f, 1f)) * 20);
    }

    public float ConvertDbToRange(float DbValue)
    {
        double result = Math.Pow(10, DbValue / 20);
        return Mathf.Clamp((float)result, 0.0001f, 1f);
    }
}
