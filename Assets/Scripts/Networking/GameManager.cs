using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager networkGameManager { get; private set; }

    private int _playerInGame = 0;
    [SerializeField] private string _prefabPath;
    [SerializeField] private Transform _spawnPos;

    private List<PlayerController> _players;
    public List<PlayerController> Players { get => _players; private set => _players = value; }

    private void Awake()
    {
        if (networkGameManager != null && networkGameManager != this)
        {
            gameObject.SetActive(false);
            return;
        }
        networkGameManager = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Players = new List<PlayerController>();
        photonView.RPC("AddPlayerInGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void AddPlayerInGame()
    {
        _playerInGame++;
        if (_playerInGame == PhotonNetwork.PlayerList.Length)
        {
            CreatePlayer();
        }
    }

    private void CreatePlayer()
    {
        var playerObject = PhotonNetwork.Instantiate(_prefabPath, _spawnPos.position, Quaternion.identity);
        var player = playerObject.GetComponent<PlayerController>();

        player.photonView.RPC("NetworkStart", RpcTarget.All, PhotonNetwork.LocalPlayer);

    }
}
