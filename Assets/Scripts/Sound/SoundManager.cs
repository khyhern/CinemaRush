using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SO_SoundRepository SoundRepoSO;
    [SerializeField] private Pool<SoundEmitter> m_SoundEmitterPool;
    [SerializeField] private string m_TestSoundName;
    [SerializeField] private AudioSource m_BGMAudioSource;

    private Dictionary<string, Sound> m_OneShotAudioDict;
    private Dictionary<string, Sound> m_BGMAudioDict;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        this.m_OneShotAudioDict = new Dictionary<string, Sound>();
        for (int s = 0; s < this.SoundRepoSO.SoundList.Length; s++)
        {
            Sound sound = this.SoundRepoSO.SoundList[s];
            this.m_OneShotAudioDict.Add(sound.SoundName, sound);
        }

        this.m_BGMAudioDict = new Dictionary<string, Sound>();
        for (int i = 0; i < this.SoundRepoSO.BGMList.Length; i++)
        {
            Sound sound = this.SoundRepoSO.BGMList[i];
            this.m_BGMAudioDict.Add(sound.SoundName, sound);
        }

    }

    private void Start()
    {
        GameObject soundEmitterParent = new GameObject("SoundEmitter Parent");
        soundEmitterParent.transform.parent = transform;
        
        m_SoundEmitterPool.Initialize(soundEmitterParent.transform);
    }

    [ContextMenu("Test Sound fx")]
    private void TestSound()
    {
        PlayOneShot("sfx_testsound", transform);
    }

    [ContextMenu("Test Runtime Sound fx")]
    private void TestRuntimeTestSound()
    {
        PlayOneShot(m_TestSoundName, transform);
    }

    [ContextMenu("Test Music")]
    private void TestMusic()
    {
        PlayBGMusic("bgm_testmusic");
    }

    /// <summary>
    /// Plays the specified sound using PlayOneShot
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayOneShot(string soundName, Transform attachedObj = null)
    {

        Sound soundToPlay;
        if (this.m_OneShotAudioDict.TryGetValue(soundName, out soundToPlay))
        {
            SoundEmitter soundEmitter = m_SoundEmitterPool.GetNextObject();

            if (attachedObj != null)
                soundEmitter.PlaySound(soundToPlay, attachedObj);
            else
                soundEmitter.PlaySound(soundToPlay, transform);
        }
        else
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

    }

    public void StopOneShot(string soundName)
    {
        if (!m_OneShotAudioDict.TryGetValue(soundName, out Sound soundToStop))
        {
            Debug.LogWarning($"Sound: {soundName} not found! Cannot stop playback.");
            return;
        }

        // Iterate through active SoundEmitters in the pool
        foreach (var soundEmitter in m_SoundEmitterPool.GetActiveObjects())
        {
            if (soundEmitter.TryGetComponent<SoundEmitter>(out SoundEmitter emitter) && emitter.IsPlayingSound(soundToStop))
            {
                emitter.StopSound();
                Debug.Log($"Stopped sound: {soundName}");
                return; // Stop once the sound is found
            }
        }

        Debug.LogWarning($"Sound: {soundName} is not currently playing!");
    }



    public void PlayBGMusic(string musicName)
    {
        Sound soundToPlay;
        if (this.m_BGMAudioDict.TryGetValue(musicName, out soundToPlay))
        {
            m_BGMAudioSource.Stop();
            m_BGMAudioSource.clip = soundToPlay.Clip;
            m_BGMAudioSource.volume = soundToPlay.Volume;
            m_BGMAudioSource.spatialBlend = soundToPlay.SpatialBlend;
            m_BGMAudioSource.pitch = soundToPlay.Pitch;
            m_BGMAudioSource.loop = true;
            m_BGMAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Music: " + musicName + " not found!");
            return;
        }
    

    }

    public void StopBGMMusic()
    {
        m_BGMAudioSource.Stop();
    }

}