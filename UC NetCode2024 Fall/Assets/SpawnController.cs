using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class SpawnController : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject _playerPrefab;
    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private NetworkVariable<int> _playerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField]
    private TMP_Text _countTxt;


    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();

        //NetworkManager.Singleton.OnClientConnectedCallback += OnConnectionEvent;
        if (IsServer)
        {
            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
        }

        _playerCount.OnValueChanged += PlayerCountChanged;


    }


    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsServer)
        {
            NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;
        }

        _playerCount.OnValueChanged -= PlayerCountChanged;
    }


    //Will fire when networkVariable value changes
    private void PlayerCountChanged(int previousValue, int newValue)
    {
        UpdateCountTextClientRpc(newValue);
    }

    /**  Legacy deprecated method just for general knowledge :)
    [ClientRpc]
    private void UpdateCountTextClientRPC()
    { 
        // do something that would be sent clients and executed
    }

    [ServerRpc]
    private void SomeServerRPC()
    { 
    
    }
    */

    [Rpc(SendTo.Everyone)]
    private void UpdateCountTextClientRpc(int newValue)
    {
        Debug.Log("Message From Client RPC");
        UpdateCountText(newValue);
    }



    private void UpdateCountText(int newValue)
    {
        _countTxt.text = $"Players : {newValue}";

    }


    private void OnConnectionEvent(NetworkManager netManager, ConnectionEventData eventData)
    {

        if (eventData.EventType == ConnectionEvent.ClientConnected)
        {
            _playerCount.Value++;
        }

        if (eventData.EventType == ConnectionEvent.ClientDisconnected)
        {
            _playerCount.Value--;

        }
    }

    public void SpawnAllPlayers()
    {

        if (!IsServer) return;

        int spawnNum = 0;
        foreach (ulong clientId in NetworkManager.ConnectedClientsIds)
        {
            //instantiate the prefab

            NetworkObject spawnedPlayerNO = NetworkManager.Instantiate(_playerPrefab, _spawnPoints[spawnNum].position, _spawnPoints[spawnNum].rotation);

            //spawn it in location based off the spawn array
            spawnedPlayerNO.SpawnAsPlayerObject(clientId);
            //then spawn the prefab

            spawnNum++;
        }

    }
}
