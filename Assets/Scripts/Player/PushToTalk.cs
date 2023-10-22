
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PushToTalk : MonoBehaviour
    {
        private Recorder _recorder;
        public InputActionReference pttKey;

        private void Awake()
        {
            if (_recorder == null)
            {
                _recorder = GetComponent<Recorder>();
            }

            
        }

        private void OnEnable()
        {
            pttKey.action.performed += EnableTalking;
            pttKey.action.canceled += DisableTalking;
        }

        private void OnDisable()
        {
            pttKey.action.performed -= EnableTalking;
            pttKey.action.canceled -= DisableTalking;
        }

        private void EnableTalking(InputAction.CallbackContext obj)
        {
            _recorder.TransmitEnabled = true;
        }
        
        private void DisableTalking(InputAction.CallbackContext obj)
        {
            _recorder.TransmitEnabled = false;
        }


        private void Update()
        {
            if (PlayerStats.Instance != null)
            {
                if (_recorder.TransmitEnabled && _recorder.VoiceDetector.Detected)
                {
                    PlayerStats.Instance.IsSpeaking = true;
                }
                else
                {
                    PlayerStats.Instance.IsSpeaking = false;
                }
            }
        }
    }
}

