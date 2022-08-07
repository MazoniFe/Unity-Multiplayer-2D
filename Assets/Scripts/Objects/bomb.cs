using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class bomb : MonoBehaviourPunCallbacks, IPunObservable
{
    private Animator animator;
    private float animationTime = 0f;


    private void Awake()
    {
        foreach(var player in GameManager.networkGameManager.Players)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.gameObject.GetComponent<Collider2D>());
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitToExplosion(6.0f));

    }


    private void Update()
    {
        animationTime += 0.35f * Time.deltaTime;
        animator.speed += animationTime * Time.deltaTime;
    }



    IEnumerator WaitToExplosion(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        Explosion();
    }


    private void Explosion()
    {
        var explosion = PhotonNetwork.Instantiate("explosion", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            

        }
        else
        {
            
        }
    }


}
