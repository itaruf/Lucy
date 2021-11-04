using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMADialog : MonoBehaviour
{
    public Dialog dialog;
    Kaleidoscope kalei;
    AudioSource source;

    void Start()
    {
        if (dialog.dialogToReadAfter != "")
        {
            StartCoroutine(TimeBeforeAction(dialog.clip.length));
        }
        else
        {
            Destroy(gameObject, dialog.clip.length);
        }

        source = GetComponent<AudioSource>();
        kalei = FindObjectOfType<Kaleidoscope>();
        kalei.audioSource.clip = source.clip;
        kalei.audioSource.volume = source.volume;
        kalei.audioSource.Play();
    }

    IEnumerator TimeBeforeAction(float time)
    {
        yield return new WaitForSeconds(time + dialog.delayBeforeNextClip);
        DialogManager.Instance.PlayDialog(dialog.dialogToReadAfter );
        kalei.audioSource.clip = null;
        Destroy(gameObject);
    }
}
