
using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UIElements;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
{
    public static FusionConnection Instance;
    
    [Header("Runner")]
    public bool connectOnAwake;
    public NetworkRunner networkRunner;

    [Header("Player Prefab")]
    [SerializeField] private NetworkObject playerPrefab;

    [Header("Sessions")]
    public GameObject sessionListCanvas;
    public Button refreshButton;
    public Transform sessionListContent;
    public GameObject sessionEntryPrefab;

    
    
    private string _playerName;
    private List<SessionInfo> _session = new();


    #region MonoBehaviour Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        if (connectOnAwake)
        {
            ConnectToSession();
        }
    }

    #endregion


    #region Network Methods


    public void ConnectToLobby(string playerName)
    {
        sessionListCanvas.SetActive(true);
        _playerName = playerName;
        
        if (networkRunner == null)
        {
            networkRunner = gameObject.AddComponent<NetworkRunner>();
        }

        networkRunner.JoinSessionLobby(SessionLobby.Shared);
    }
    
    public async void ConnectToSession(string sessionName)
    {
        sessionListCanvas.SetActive(false);
        
        if (networkRunner == null)
        {
            networkRunner = gameObject.AddComponent<NetworkRunner>();
        }

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName,
            Scene = 0,
            PlayerCount = 2,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    
    public void ConnectToSession()
    {
        int randomInt = Random.Range(1000, 9999);
        string sessionName = "room-" + randomInt;

        ConnectToSession(sessionName);
    }

    public void RefreshSessionListUI()
    {
        foreach (Transform child in sessionListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (SessionInfo session in _session)
        {
            if (session.IsVisible)
            {
                GameObject entry = Instantiate(sessionEntryPrefab.gameObject, sessionListContent);
                SessionEntryPrefab script = entry.GetComponent<SessionEntryPrefab>();
                script.sessionName.text = session.Name;
                script.playerCount.text = session.PlayerCount + " / " + session.MaxPlayers;

                if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                {
                    script.joinButton.interactable = false;
                }
                else
                {
                    script.joinButton.interactable = true;
                }
            }
        }
    }

    #endregion
    
    
    #region INetworkRunnerCallbacks Methods

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("On Player Joined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("On Player Left");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // Debug.Log("On Input");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("On Input Missing");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("On Shutdown");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("On Connected To Server");
        NetworkObject playerObject = runner.Spawn(playerPrefab, Vector3.zero);
        runner.SetPlayerObject(runner.LocalPlayer, playerObject);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("On Disconnected From Server");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("On Connect Request");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("On Connect Failed");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("On User Simulation Message");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("On Session List Updated");
        _session.Clear();
        _session = sessionList;
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("On Custom Authentication Response");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("On Host Migration");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.Log("On Reliable Data Received");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("On Scene Load Done");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("On Scene Load Start");
    }

    #endregion


    #region Util Methods

    public string GetPlayerName()
    {
        return _playerName;
    }

    #endregion
}
