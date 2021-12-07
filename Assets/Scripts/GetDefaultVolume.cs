using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDefaultVolume : MonoBehaviour
{
    public AudioSource volumeTarget;
    public Slider target;

    void Start()
    {
        target.value = volumeTarget.volume;
    }
}
