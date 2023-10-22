
using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UIElements;
using UnityEngine.UI;


public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
{
    public static FusionConnection Instance;
    
    private string _playerName;
    
    [Header("Runner")]
    public bool connectOnAwake;
    public NetworkRunner networkRunner;

    [Header("Player Prefab")]
    [SerializeField] private NetworkObject playerPrefab;

    [Header("Session Creation")]
    public GameObject createSessionCanvas;
    public TMP_InputField sessionNameInput;
    public TMP_InputField passcodeInput;

    [Header("Session Join")]
    public GameObject joinSessionCanvas;
    public TMP_InputField joinPasscodeInput;
    public GameObject invalidText;
    
    [Header("Session List")]
    public GameObject sessionListCanvas;
    public Button refreshButton;
    public Transform sessionListContent;
    public GameObject sessionEntryPrefab;
    
    
    private string _currentAttemptSessionCode;
    private string _currentAttemptSessionName;
    private bool _firstLobby = false;
    
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
    
    public void ConnectToSession(string sessionName, string sessionCode)
    {
        _currentAttemptSessionName = sessionName;
        _currentAttemptSessionCode = sessionCode;
        
        joinSessionCanvas.SetActive(true);
    }

    public async void FinishConnectingToSession()
    {
        if (_currentAttemptSessionCode == joinPasscodeInput.text)
        {
            sessionListCanvas.SetActive(false);
            createSessionCanvas.SetActive(false);
            joinSessionCanvas.SetActive(false);
        
            if (networkRunner == null)
            {
                networkRunner = gameObject.AddComponent<NetworkRunner>();
            }

            await networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = _currentAttemptSessionName,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        else
        {
            invalidText.SetActive(true);
        }
        
        
    }
    
    public async void ConnectToSession()
    {
        string sessionName = sessionNameInput.text;
        string sessionCode = passcodeInput.text;

        SessionProperty code = sessionCode;
        Dictionary<string, SessionProperty> sessionProperties = new Dictionary<string, SessionProperty>();
        sessionProperties.Add("sessionCode", code);

        sessionListCanvas.SetActive(false);
        createSessionCanvas.SetActive(false);
        
        if (networkRunner == null)
        {
            networkRunner = gameObject.AddComponent<NetworkRunner>();
        }

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName,
            SessionProperties = sessionProperties,
            PlayerCount = 2,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
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
                script.sessionKey = session.Properties.GetValueOrDefault("sessionCode").PropertyValue as string;

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

        if (_firstLobby == false)
        {
            RefreshSessionListUI();
            _firstLobby = true;
        }
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
