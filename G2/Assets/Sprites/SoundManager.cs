using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] public AudioSource SkillSound;
    [SerializeField] public List<AudioSource> SkillSoundList;
    public static SoundManager instance;
    private void Start()
    {
        instance = this;
    }

    public void PlaySound(int index)
    {

    }
}
