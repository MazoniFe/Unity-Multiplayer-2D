using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //SINGLETON
    public static NetworkManager networkInstantiate { get; private set; }

    private void Awake()
    {
        if(networkInstantiate != null && networkInstantiate != this)
        {
            gameObject.SetActive(false);
            return;
        }
        networkInstantiate = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connection Success");
    }

    public void NetworkCreateRoom(string _roomName)
    {
        PhotonNetwork.CreateRoom(_roomName);
    }
    public void NetworkJoinRoom(string _roomName)
    {
        PhotonNetwork.JoinRoom(_roomName);
    }
    public void NetworkLeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    [PunRPC]
    public void NetworkStartMatch(string _sceneName)
    {
        PhotonNetwork.LoadLevel(_sceneName);
    }

    public void ChangeNickname(string _nickname)
    {
        PhotonNetwork.NickName = _nickname;
    }

    public string GetPlayerList()
    {
        var list = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            list += player.NickName + "\n";
        }
        return list;
    }

    public bool GetPlayerMaster()
    {
        return PhotonNetwork.IsMasterClient;
    }
}
