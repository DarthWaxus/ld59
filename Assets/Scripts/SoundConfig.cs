using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "Game/SoundConfig", order = 0)]
public class SoundConfig : ScriptableObject
{
    public AudioClip clip;
    public float minPitch = 1;
    public float maxPitch = 1;
    public float volume = 1;
}