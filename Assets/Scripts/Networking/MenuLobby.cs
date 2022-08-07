using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MenuLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _PlayerList;
    [SerializeField] private Button _startGame;

    [PunRPC]
    public void UpdateList()
    {
        _PlayerList.text = NetworkManager.networkInstantiate.GetPlayerList();
        _startGame.interactable = NetworkManager.networkInstantiate.GetPlayerMaster();
    }

    

}
