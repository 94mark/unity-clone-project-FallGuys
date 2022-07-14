using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DevionGames
{
    [CreateAssetMenu(fileName = "Audio Playlist", menuName = "Devion Games/Utilities/Audio Playlist")]
    [System.Serializable]
    public class AudioPlaylist : ScriptableObject
    {
        [SerializeField]
        protected List<AudioClip> m_Clips;

        public AudioClip this[int index]
        {
            get { return this.m_Clips[index]; }
        }

        public int Count
        {
            get
            {
                return this.m_Clips.Count;
            }
        }

    }
}