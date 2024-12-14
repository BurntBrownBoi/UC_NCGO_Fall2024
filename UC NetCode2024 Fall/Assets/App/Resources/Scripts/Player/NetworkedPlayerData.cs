using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NewtorkedPlayerData : NetworkBehaviour
{
    public NetworkList<PlayerInfoData> _allConnectedPlayers;
    private int _players = -1;
    private ulong _serverLocalID;

    private Color[] _PlayerColors = new Color[]
    {
        Color.blue, Color.magenta, Color.cyan, Color.yellow, Color.white

    };

    private void Awake()
    {
        _allConnectedPlayers = new NetworkList<PlayerInfoData>(readPerm: NetworkVariableReadPermission.Everyone);
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(!IsServer) return;
        NetworkManager.Singleton.OnConnectionEvent += OnConnectionevents;
        _serverLocalID = NetworkManager.ServerClientId;


    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnConnectionEvent -= OnConnectionevents;
        }

        base.OnNetworkDespawn();
    }


    private void OnConnectionevents(NetworkManager netManager, ConnectionEventData eventData)
    {

        if (eventData.EventType == ConnectionEvent.ClientConnected)
        {
            // when client connects create data
            CreateNewClientData(eventData.ClientId);
        }

        if (eventData.EventType == ConnectionEvent.ClientDisconnected)
        {

            _players--;

        }


    }



    private void CreateNewClientData(ulong clientID)
    {
        // create new player info
        PlayerInfoData playerInfoData = new PlayerInfoData();

        // check to see if servr matches parameter clientID

        if (_serverLocalID == clientID)
        {
            //if we are the host assume we are always ready!
            playerInfoData._isPlayerReady = true;
        }
        else
        {
            //clients are set to false
            playerInfoData._isPlayerReady = false;
        }

        _players++;

        playerInfoData._colorId = _PlayerColors[_players];

        //add to netList

        _allConnectedPlayers.Add(playerInfoData);
    }


    public void RemovePlayerData(PlayerInfoData playerInfoData)
    {
        _allConnectedPlayers.Remove(playerInfoData);
    }

    public PlayerInfoData FindPlayerInfoData(ulong clientID)
    {
        return _allConnectedPlayers[FindPlayerIndex(clientID)];

    }

    private int FindPlayerIndex(ulong clientID)
    {
        int myMatch = -1;

        for (int i = 0; i < _allConnectedPlayers.Count; i++)
        {

            if (clientID == _allConnectedPlayers[i]._clientId)
            {
                myMatch = i;
            }

        }

        return myMatch;

    }

    private void UpdateReadyClient(ulong clientID, bool isReady)
    {
        int idx = FindPlayerIndex(clientID);

        if (idx == -1) { return; }


        //Grab info, change it, and pass it back to the networkList
        PlayerInfoData playerInfo = new PlayerInfoData();
        //copy data
        playerInfo = _allConnectedPlayers[idx];
        //change status
        playerInfo._isPlayerReady = isReady;
        //update new status to list
        _allConnectedPlayers[idx] = playerInfo;
    }
}
