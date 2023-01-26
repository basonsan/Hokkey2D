using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckMove : MonoBehaviour
{
    [SerializeField] float slowRate;
    [SerializeField] BoxCollider2D bc2d;
    private Transform player;
    private bool isPlayerTakePuck;
    private float speedPuck;
    private Vector3 target;
    private Vector3 velocity;
    //private float speed;
    public event Action<PlayerControl> OntrigerPlaer;
    

    private void Start()
    {
        isPlayerTakePuck = false;
    }
    private void Update()
    {
        if (isPlayerTakePuck)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, 1f);
        }
        if (speedPuck > 0)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target, speedPuck * Time.deltaTime);
            //GetComponent<Rigidbody2D>().MovePosition(transform.position + target * Time.deltaTime);
            transform.position += velocity * Time.deltaTime;
            speedPuck -= Time.deltaTime * slowRate;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent<PlayerControl>(out PlayerControl playerControl);
        if (playerControl)
        {
            isPlayerTakePuck = true;
            player = playerControl.PuckPoint;
            OntrigerPlaer.Invoke(playerControl);
            playerControl.StrikePuck += SetParametrPuck;
            bc2d.enabled = false;
        }
        collision.transform.root.TryGetComponent<BorderStadion>(out BorderStadion borderStadion);
        if (borderStadion)
        {
            velocity *= -1;
        }
        Debug.Log(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent<PlayerControl>(out PlayerControl playerControl);
        if (playerControl)
        {
            isPlayerTakePuck = false;
            playerControl.StrikePuck -= SetParametrPuck;
            bc2d.enabled = true;

        }
    }

    private void SetParametrPuck(float speed, Gate gate)
    {
        speedPuck = speed;
        target = gate.transform.position;
        velocity = target - transform.position;
    }

}
