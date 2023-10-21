
using Fusion;
using Player.Avatar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerStats : NetworkBehaviour
    {
        public static PlayerStats Instance;
        
        [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }
        
        [Networked(OnChanged = nameof(UpdateHat))] public int HatIndex { get; set; }
        
        [Networked(OnChanged = nameof(UpdateSpeakingIndicator))] public NetworkBool IsSpeaking { get; set; }

        [SerializeField] private TMP_Text playerNameLabel;
        [SerializeField] private Transform playerHead;
        
        private GameObject _currentHat = null;

        public Image speakingIndicator;


        private void Start()
        {
            if (HasStateAuthority)
            {
                PlayerName = FusionConnection.Instance.GetPlayerName();
                if (Instance == null)
                {
                    Instance = this;
                }
            }
        }

        protected static void UpdatePlayerName(Changed<PlayerStats> changed)
        {
            changed.Behaviour.playerNameLabel.text = changed.Behaviour.PlayerName.ToString();
        }

        protected static void UpdateHat(Changed<PlayerStats> changed)
        {
            int hatIndex = changed.Behaviour.HatIndex;

            GameObject curHat = changed.Behaviour._currentHat;
            GameObject hat = Hats.hats[hatIndex];
            
            if (curHat != null)
            {
                Destroy(curHat);
            }

            GameObject newHat = Instantiate(hat, changed.Behaviour.playerHead);
            newHat.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            newHat.transform.localScale = Vector3.one;
            newHat.GetComponent<Collider>().enabled = false;
            
            changed.Behaviour._currentHat = newHat;
        }
        
        protected static void UpdateSpeakingIndicator(Changed<PlayerStats> changed)
        {
            bool isSpeaking = changed.Behaviour.IsSpeaking;
            Image speakingIndicator = changed.Behaviour.speakingIndicator;
            
            if (isSpeaking)
            {
                speakingIndicator.enabled = true;
            }
            else
            {
                speakingIndicator.enabled = false;
            }
        }
    }
}

