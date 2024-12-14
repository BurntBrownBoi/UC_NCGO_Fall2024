using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private Button _startBttn, _leaveBttn, _readyBttn;
    [SerializeField] private GameObject _panelPrefab; // The prefab we place inside the contents 
    [SerializeField] private GameObject _ContentGO; //Where we are spawning panelPrefabs to 
    [SerializeField] private TMP_Text rdyTxt; // update status to user 

    /// list of network players
    [SerializeField] private NetworkedPlayerData _networkPlayers;


    private List<GameObject> _PlayerPanels = new List<GameObject>();

    private ulong _myLocalClientID;

    private bool isReady = false;
    private ulong _myServerID;


    //populate Panels

    [ContextMenu("PopulateLabel")]
    private void PopulateLabels()
    {
        //clear Panels
        ClearPlayerPanel();

        // loop all player info from allnetowrked players and then create new panels

        bool allReady = true; //used for logic on server

        foreach (PlayerInfoData playerData in _networkPlayers._allConnectedPlayers)
        {
            // instatnitate

            GameObject newPlayerPanel = Instantiate(_panelPrefab, _ContentGO.transform);
            PlayerLabel _playerLabel = newPlayerPanel.GetComponent<PlayerLabel>();

            //sub to kick events on panels

            _playerLabel.onKickClicked += KickUserBttn;

            // depending on client vs sserver, we are going to show/hide kick bttns

            if(IsServer && playerData._clientId != _myServerID)
            {
                //esnure that we are the host and set active kick buttons
                _playerLabel.setKickActive(true);

                //where we are at itm ensuyre servers ready button is hiddem. we assume server is awlays ready
                _readyBttn.GameObject().SetActive(false);  
            }
            else
            {
                // ensure clients dont have set kicked buttons visible but ready is visible
                _playerLabel.setKickActive(false);
                _readyBttn.GameObject().SetActive(true);
            }


            // display info to ui
            _playerLabel.SetPlayerLabelName(playerData._clientId);
            _playerLabel.SetReady(playerData._isPlayerReady);
            _playerLabel.SetPlayerColor(playerData._colorId);
            _PlayerPanels.Add(newPlayerPanel);

            if(playerData._isPlayerReady == false)
            {
                allReady = false;
            }


        }


        //Check if everyone is ready, host would see if its ready or not
        if (IsServer)
        {
            if (allReady)
            {
                if (_networkPlayers._allConnectedPlayers.Count > 1)
                {
                    rdyTxt.text = "Ready to start";
                    _startBttn.gameObject.SetActive(true);
                }
                else
                {
                    rdyTxt.text = "Empty Lobby";
                }

            }
            else
            {
                _startBttn.gameObject.SetActive(false);
                rdyTxt.text = "waiting for ready players";
            }
        }

    }

    private void KickUserBttn(ulong obj)
    {
        throw new NotImplementedException();
    }

    private void ClearPlayerPanel()
    {

        foreach (GameObject panel in _PlayerPanels)
        {
            Destroy(panel);
        }

        _PlayerPanels.Clear();

    }


    //Kick Users


}   