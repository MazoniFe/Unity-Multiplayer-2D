using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryRoom : MonoBehaviour
{
    [SerializeField]private Text _playerName;
    [SerializeField]private Text _roomName;
    [SerializeField] private Text _roomList;


    public void CreateRoom()
    {
        NetworkManager.networkInstantiate.ChangeNickname(_playerName.text);
        NetworkManager.networkInstantiate.NetworkCreateRoom(_roomName.text);
    }
    public void JoinRoom()
    {
        NetworkManager.networkInstantiate.ChangeNickname(_playerName.text);
        NetworkManager.networkInstantiate.NetworkJoinRoom(_roomName.text);
    }

    public void ShowRooms()
    {
        
    }
}
