
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class RefreshButton : MonoBehaviour
    {
        private Button _refreshButton;


        private void Awake()
        {
            if (_refreshButton == null)
            {
                _refreshButton = GetComponent<Button>();
            }
            
            _refreshButton.onClick.AddListener(Refresh);
        }
        
        private void OnDestroy()
        {
            _refreshButton.onClick.RemoveAllListeners();
            _refreshButton = null;
        }

        private void Refresh()
        {
            StartCoroutine(RefreshWait());
        }

        private IEnumerator RefreshWait()
        {
            _refreshButton.interactable = false;
            FusionConnection.Instance.RefreshSessionListUI();
            
            yield return new WaitForSeconds(3f);
            _refreshButton.interactable = true;
        }
    }
}

