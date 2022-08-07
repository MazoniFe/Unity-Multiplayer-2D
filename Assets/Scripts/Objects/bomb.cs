using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class bomb : MonoBehaviourPunCallbacks, IPunObservable
{
    private Animator animator;
    private float animationTime = 0f;
    private GameObject explosion;


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
        StartCoroutine(WaitToExplosion(4.0f));

    }


    private void Update()
    {
        animationTime += 0.85f * Time.deltaTime;
        animator.speed += animationTime * Time.deltaTime;
    }



    IEnumerator WaitToExplosion(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
        Explosion();
    }


    private void Explosion()
    {
        explosion = PhotonNetwork.Instantiate("explosion", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }


}
