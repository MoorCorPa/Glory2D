using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Home
{
    public class mainCanvas : MonoBehaviour
    {
        public AudioMixer 音乐;
        public AudioMixer 音效;

        private void Start()
        {
            存档管理器.读取音量(音乐, 音效);
        }

    }
}