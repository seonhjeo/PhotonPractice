
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class SessionEntryPrefab : MonoBehaviour
    {
        public TMP_Text sessionName;
        public string sessionKey;
        public TMP_Text playerCount;
        public Button joinButton;

        private void Awake()
        {
            joinButton.onClick.AddListener(JoinSession);
        }

        private void Start()
        {
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }


        private void OnDestroy()
        {
            joinButton.onClick.RemoveAllListeners();
        }


        private void JoinSession()
        {
            FusionConnection.Instance.ConnectToSession(sessionName.text, sessionKey);
        }
    }
}

