
using Cinemachine;
using Fusion;
using StarterAssets;
using UnityEngine;

namespace Player
{
    public class GetPlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform playerCameraRoot;

        private void Start()
        {
            NetworkObject thisObject = GetComponent<NetworkObject>();
            if (thisObject.HasStateAuthority)
            {
                GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;

                GetComponent<ThirdPersonController>().enabled = true;
            }
        }
    }
}

