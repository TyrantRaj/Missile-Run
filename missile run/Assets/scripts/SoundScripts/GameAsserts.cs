using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsserts : MonoBehaviour
{
    private static GameAsserts _i;

    public static GameAsserts i {
        get{
            if (_i == null) _i = Instantiate(Resources.Load<GameAsserts>("GameAsserts"));
            return _i;
        }
    }

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }



}
