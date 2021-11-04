using UnityEngine;

public class Kaleidoscope : MonoBehaviour
{
    public RectTransform[] imagesLayers;
    public float speed = 0.1f;

    [Header("Audio")]
    public float[] multipleLayerWith;
    public float minSize = 1;
    public float maxSize = 1.5f;
    public float divider = 180;
    [Space(20)]
    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    private float clipLoudness;
    private float[] clipSampleData;
    void Awake()
    {
        clipSampleData = new float[sampleDataLength];
    }
    void Update()
    {
        //ROTATE

        for (int i = 0; i < imagesLayers.Length; i++)
        {
            imagesLayers[i].Rotate(0, 0, (1 + i) * 2 * Time.deltaTime * speed);
        }

        //AUDIO

        if(audioSource == null)
        {
            for (int i = 0; i < imagesLayers.Length; i++)
            {
                imagesLayers[i].localScale = Vector3.one * minSize;
            }
            return;
        }
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
                float clamp =  1+ clipLoudness / divider;

                if (clamp > maxSize)
                    clamp = maxSize;
                else if (clamp < minSize)
                    clamp = minSize; 

                //clamp = Mathf.Clamp(clipLoudness, minSize, maxSize);
                for (int i = 0; i < imagesLayers.Length; i++)
                {
                    Vector3 newScale;
                    if (multipleLayerWith[i] == 0)
                        newScale = Vector3.one;
                    else
                        newScale = Vector3.one * clamp * multipleLayerWith[i];
                    imagesLayers[i].localScale = Vector3.Lerp(imagesLayers[i].localScale, newScale, Time.deltaTime);
                }
            }
        }

    }
}