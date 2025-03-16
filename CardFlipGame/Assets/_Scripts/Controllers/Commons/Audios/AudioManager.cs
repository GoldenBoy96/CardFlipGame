using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;


    [Header("Data")]
    [SerializeField] private float loadingTimeOut = 1f;
    [SerializeField] private List<AudioData> audioDatas = new();


    public static AudioManager Instance { get => instance; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadAudioList();
    }

    private void LoadAudioList()
    {
        foreach (AudioData audioData in audioDatas)
        {
            if (audioData.AudioSource == null)
            {
                audioData.AudioSource = gameObject.AddComponent<AudioSource>();
            }
            audioData.AudioSource.clip = audioData.Clip;
            audioData.AudioSource.volume = audioData.Volume;
            audioData.AudioName = audioData.Clip.name;
        }
    }

    public void PlayAudio(string audioName)
    {
        var audioData = audioDatas.FirstOrDefault((audioData) => audioData.AudioName.Equals(audioName));
        if (audioData == null)
        {
            Debug.Log("Audio not found!");
            return;
        }
        if (audioData.AudioSource.clip.loadState == AudioDataLoadState.Loaded)
        {
            audioData.AudioSource.Play();
        }
        else
        {
            StartCoroutine(PlayAudioCoroutine(audioData));
        }
    }

    IEnumerator PlayAudioCoroutine(AudioData audioData)
    {
        float delayLoadingTime = 0f;
        audioData.AudioSource.clip.LoadAudioData();
        while (!audioData.AudioSource.clip.loadState.Equals(AudioDataLoadState.Loaded))
        {
            if (delayLoadingTime > loadingTimeOut)
            {
                Debug.Log($"Can not load audio {audioData.AudioName}!");
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
                delayLoadingTime += 0.01f;
                Debug.Log(audioData.AudioSource.clip.loadState + " |" + delayLoadingTime + " | " + loadingTimeOut);
                
            }
        }
        if (audioData.AudioSource.clip.loadState.Equals(AudioDataLoadState.Loaded))
        {
            audioData.AudioSource.Play();
            audioData.AudioSource.time = delayLoadingTime;
        }
    }

    public void StopAudio(AudioData audioData)
    {
        try
        {
            audioData.AudioSource.Stop();
        }
        catch
        {
            Debug.Log($"Can not stop audio {audioData.AudioName}!");
        }
    }

}

[Serializable]
public class AudioData
{
    [SerializeField] private AudioClip clip;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float volume;
    [SerializeField]AudioSource audioSource;
    [SerializeField] string audioName;
    public AudioClip Clip { get => clip; set => clip = value; }
    public float Volume { get => volume; set => volume = value; }
    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    public string AudioName { get => audioName; set => audioName = value; }

}
