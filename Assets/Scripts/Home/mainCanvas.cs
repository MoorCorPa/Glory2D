using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Assets.Scripts.Home
{
    public class mainCanvas : MonoBehaviour
    {

        [SerializeField] private UnityEvent 呼叫按键;
        public AudioMixer 音乐;
        public AudioMixer 音效;

        private void Start()
        {
            存档管理器.读取音量(音乐, 音效);
            呼叫按键.Invoke();
        }

    }
}