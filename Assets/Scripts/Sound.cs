using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sound : MonoBehaviour
{
    public static Sound I;

    public List<SoundConfig> configs;

    public int poolSize = 10;

    private Dictionary<string, SoundConfig> configMap;
    private List<AudioSource> sources;
    
    public bool soundEnabled = true;

    private void Awake()
    {
        I = this;

        configMap = new Dictionary<string, SoundConfig>();
        foreach (var config in configs)
            configMap.TryAdd(config.name, config);

        sources = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            var go = new GameObject($"AudioSource_{i}");
            go.transform.parent = transform;

            var source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;

            sources.Add(source);
        }
    }

    public void Play(string soundName)
    {
        if(!soundEnabled) return;
        if (!configMap.TryGetValue(soundName, out var config)) return;

        var source = GetFreeSource();
        if (source == null) return;
        source.pitch = Random.Range(config.minPitch, config.maxPitch);
        source.volume = config.volume;
        source.PlayOneShot(config.clip);
    }

    private AudioSource GetFreeSource()
    {
        foreach (var audioSource in sources)
            if (!audioSource.isPlaying)
                return audioSource;

        return null;
    }
}