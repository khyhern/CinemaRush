using UnityEngine;
using UnityEngine.Audio;

public class SoundEmitter : MonoBehaviour
{
    private static int EXECUTE_EVERY_N_FRAMES = 5;

    private AudioSource m_AudioSource;
    private Transform m_AttachedObj;
    private Sound m_CurrentSound;
    
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(Sound soundToPlay, Transform attachObj)
    {
        m_AttachedObj = attachObj;
        m_CurrentSound = soundToPlay;
        
        m_AudioSource.volume = soundToPlay.Volume;
        m_AudioSource.spatialBlend = soundToPlay.SpatialBlend;
        m_AudioSource.pitch = soundToPlay.Pitch;

        m_AudioSource.PlayOneShot(soundToPlay.Clip);

        Invoke("ResetAttachedObject", soundToPlay.Clip.length);
    }
    
    public void StopSound()
    {
        m_AudioSource.Stop();
        m_CurrentSound = null;
    }

    public bool IsPlayingSound(Sound sound)
    {
        return m_AudioSource.isPlaying && m_CurrentSound == sound;
    }

    private void ResetAttachedObject()
    {
        m_AttachedObj = null;
        m_CurrentSound = null;
    }

    private void Update()
    {

        // Restricts update to every N frames
        if (Time.frameCount % EXECUTE_EVERY_N_FRAMES == 0)
        {
            if (m_AttachedObj != null)
            {
                transform.position = m_AttachedObj.position;
            }
        }
        
    }
}