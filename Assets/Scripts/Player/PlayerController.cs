using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using Photon.Realtime;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float forceJump;
    [SerializeField] private bool itsGrounded;
    private float moveVertical = 0.0f;
    private float moveHorizontal = 0.0f;
    private SpriteRenderer playerSprite;
    private Rigidbody2D _rigidbody;

    private Vector2 networkPosition;
    private string _bombPath = "bomb";
    private bool canSpawnBomb = true;
    private bool isDied =false;

    private Player _photonPlayer;
    private int _id;

    private bool itsLocalPlayer = true;


    Animator _animator;
    private string currentState;
    //Animation States
    const string PLAYER_IDLE = "idle";
    const string PLAYER_RUN = "run";
    const string PLAYER_JUMP = "jump";
    const string PLAYER_DIE = "die";


    [PunRPC]
    public void NetworkStart(Player player)
    {
        _photonPlayer = player;
        _id = player.ActorNumber;
        GameManager.networkGameManager.Players.Add(this);
        if (!photonView.IsMine) {
            itsLocalPlayer = false;
            playerSprite.sortingOrder = 1;
        }

    }

    private void Awake()
    {
        isDied = false;
        _animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!itsLocalPlayer) return;
        Inputs();
    }

    private void FixedUpdate()
    {
        if (!itsLocalPlayer) return;
        if (isDied)
        {
            photonView.RPC("ChangeAnimationState", RpcTarget.All, PLAYER_DIE);
            return;
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        if (moveHorizontal != 0 && isDied == false)
        {
            MovePlayer();
            this.GetComponent<PhotonView>().RPC("ChangeAnimationState", RpcTarget.All, PLAYER_RUN);
        }
        else this.GetComponent<PhotonView>().RPC("ChangeAnimationState", RpcTarget.All, PLAYER_IDLE);
    }

    private void Inputs()
    {
        if (Input.GetKey(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnBomb();
        }
    }

    private void SpawnBomb()
    {
        if (!canSpawnBomb) return;
        var bomb = PhotonNetwork.Instantiate(_bombPath, new Vector2(transform.position.x, transform.position.y + 0.8f), Quaternion.identity);
        canSpawnBomb = false;
        StartCoroutine(SpawnBombCd(2.0f));

    }

    private void MovePlayer()
    {
        if (moveHorizontal > 0) playerSprite.flipX = false;
        else playerSprite.flipX = true;
        transform.Translate(moveHorizontal * Time.deltaTime * moveSpeed, 0, 0);
    }

    private void Jump()
    {
        if (_rigidbody.velocity.y != 0 || isDied) return;
        else _rigidbody.AddForce(transform.up * forceJump, ForceMode2D.Impulse);
    }

    [PunRPC]
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        _animator.Play(newState);
        currentState = newState;
    }


    IEnumerator SpawnBombCd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSpawnBomb = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            isDied = true;
            ChangeAnimationState(PLAYER_DIE);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerSprite.flipX);
            stream.SendNext(_animator.GetBool("running"));
            stream.SendNext(isDied);

        }
        else
        {
            playerSprite.flipX = (bool)stream.ReceiveNext();
            _animator.SetBool("running", (bool)stream.ReceiveNext());
            isDied = (bool)stream.ReceiveNext();
        }
    }
}
