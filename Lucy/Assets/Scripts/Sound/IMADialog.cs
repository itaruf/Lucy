using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMADialog : MonoBehaviour
{
    public Dialog dialog;
    AudioSource kalei;
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
        kalei = FindObjectOfType<Kaleidoscope>().audioSource = source;
    }

    IEnumerator TimeBeforeAction(float time)
    {
        yield return new WaitForSeconds(time);
        DialogManager.Instance.PlayDialog(dialog.dialogToReadAfter + dialog.delayBeforeNextClip);
        kalei = null;
        Destroy(gameObject);
    }
}
