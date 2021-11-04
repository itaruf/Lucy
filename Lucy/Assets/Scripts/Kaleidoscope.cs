using UnityEngine;

public class Kaleidoscope : MonoBehaviour
{
    public RectTransform[] imagesLayers;
    public float speed = 0.1f;


    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    private float clipLoudness;
    private float[] clipSampleData;
    void Awake()
    {

        if (!audioSource)
        {
            Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
        }
        clipSampleData = new float[sampleDataLength];

    }
    void Update()
    {
        for (int i = 0; i < imagesLayers.Length; i++)
        {
            imagesLayers[i].Rotate(0, 0, (1 + i) * 2 * Time.deltaTime * speed);
        }

        return;
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
                for (int i = 0; i < imagesLayers.Length; i++)
                {
                    float clamp = (clipLoudness * 2 / 1000 * (i+1));
                    imagesLayers[i].localScale = Vector3.one * clamp;
                }
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
        }

    }
}
