using System.Collections;
using UnityEngine;

public class SoundTester : MonoBehaviour
{
    public string SoundToPlay;
    public int PlayEveryNSeconds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PlayTestSoundCoroutine());
    }
    public void EmptyFunctionTest()
    {

    }
    public void TestSound(string sfx)
    {
        SoundManager.Instance.PlayOneShot(sfx, transform);

    }
    private IEnumerator PlayTestSoundCoroutine()
    {
        yield return null;

        while (true)
        {
            SoundManager.Instance.PlayOneShot(SoundToPlay, transform);
            yield return new WaitForSeconds(PlayEveryNSeconds);
        }
    }
}
