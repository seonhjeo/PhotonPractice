
using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;


public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
{
    public bool connectOnAwake;
    
    [HideInInspector]
    public NetworkRunner networkRunner;

    [SerializeField] private NetworkObject playerPrefab;

    

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (connectOnAwake)
        {
            ConnectToRunner();
        }
    }

    #endregion


    #region Network Methods

    private async void ConnectToRunner()
    {
        if (networkRunner == null)
        {
            networkRunner = gameObject.AddComponent<NetworkRunner>();
        }

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "test",
            Scene = 0,
            PlayerCount = 2,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
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

    
}
