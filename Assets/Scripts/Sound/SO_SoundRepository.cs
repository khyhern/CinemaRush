using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SoundRepo", menuName = "Scriptable Objects/SO_SoundRepo")]
public class SO_SoundRepository : ScriptableObject
{
    public Sound[] SoundList;
    public Sound[] BGMList;
}