
using System;
using System.Collections;
using Fusion;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerStats : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }

        [SerializeField] private TMP_Text playerNameLabel;

        
        private void Start()
        {
            if (HasStateAuthority)
            {
                PlayerName = FusionConnection.instance.GetPlayerName();
            }
        }

        protected static void UpdatePlayerName(Changed<PlayerStats> changed)
        {
            changed.Behaviour.playerNameLabel.text = changed.Behaviour.PlayerName.ToString();
        }
    }
}

