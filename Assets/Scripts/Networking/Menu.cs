using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [SerializeField] private EntryRoom _entryRoom;
    [SerializeField] private MenuLobby _menuLobby;

    private void Start()
    {
        _entryRoom.gameObject.SetActive(false);
        _menuLobby.gameObject.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        _entryRoom.gameObject.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        SwitchMenu(_menuLobby.gameObject);
        _menuLobby.photonView.RPC("UpdateList", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _menuLobby.photonView.RPC("UpdateList", RpcTarget.All);
    }

    public void SwitchMenu(GameObject _menu)
    {
        _entryRoom.gameObject.SetActive(false);
        _menuLobby.gameObject.SetActive(false);
        _menu.SetActive(true);
    }

    public void LeaveRoom()
    {
        NetworkManager.networkInstantiate.NetworkLeaveLobby();
        SwitchMenu(_entryRoom.gameObject);
    }
    public void StartGame(string _sceneName)
    {
        NetworkManager.networkInstantiate.photonView.RPC("NetworkStartMatch",RpcTarget.All,_sceneName);
    }
}
