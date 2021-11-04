using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackChild : MonoBehaviour
{
    Image img;
    public void Active()
    {
        StopAllCoroutines();
        img = GetComponent<Image>();
        StartCoroutine(PlayerTouch());
    }

    IEnumerator PlayerTouch()
    {
        Color alpha100 = new Vector4(255, 255, 255, 100);
        Color alpha0 = new Vector4(255, 255, 255, 0);
        img.color = alpha100;
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 10;
            Debug.Log(time);
            img.color = Color.Lerp(alpha100, alpha0, time);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
