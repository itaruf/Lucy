using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f;
    public string dialogToReadAfter;
    public float delayBeforeNextClip = 0.5f;
}
