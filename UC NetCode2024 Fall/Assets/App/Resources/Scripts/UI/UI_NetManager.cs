using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class UI_NetManager : NetworkBehaviour
{

    [SerializeField] private Button _serverBttn, _clientBttn, _hostBttn;

    // Start is called before the first frame update
    void Start()
    {
        _hostBttn.onClick.AddListener(HostClick);
        _clientBttn.onClick.AddListener(ClientClick);
        _serverBttn.onClick.AddListener(ServerClick);
    }




    // Update is called once per frame
    private void ServerClick()
    {
        NetworkManager.Singleton.StartServer();
        this.gameObject.SetActive(false);
    }
    private void ClientClick()
    {
        NetworkManager.Singleton.StartClient();
        this.gameObject.SetActive(false);
    }
    private void HostClick()
    {
        NetworkManager.Singleton.StartHost();
        this.gameObject.SetActive(false);
    }
}
