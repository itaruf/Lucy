using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public Dialog[] dialogs;
    public static DialogManager Instance;
    public GameObject soundPrefab;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayDialog(string name)
    {
        Dialog s = Array.Find(dialogs, sound => sound.name == name);

        GameObject obj2D = Instantiate(soundPrefab);
        obj2D.name = name;
        AudioSource source2D = obj2D.GetComponent<AudioSource>();

        obj2D.name = name;
        obj2D.GetComponent<IMADialog>().dialog = s;

        source2D.clip = s.clip;
        source2D.volume = s.volume;

        source2D.PlayOneShot(source2D.clip);
    }
}
