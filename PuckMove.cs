using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckMove : MonoBehaviour
{
    [SerializeField] float slowRate;
    [SerializeField] BoxCollider2D bc2d;
    //начинаем указывать слева снизу
    [SerializeField] private Transform[] pointStadion;

    private Transform player;
    private bool isPlayerTakePuck;
    private float speedPuck;
    private Vector3 target;
    private Vector3 velocity;
    private bool isVelositySwapX;
    private bool isVelositySwapY;
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
            transform.position += velocity * speedPuck * Time.deltaTime;
            //speedPuck -= Time.deltaTime * slowRate * Mathf.Sqrt(speedPuck);
            speedPuck -=  Mathf.Sqrt(speedPuck * Time.deltaTime * slowRate);



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
            speedPuck = 0;
        }
        collision.transform.root.TryGetComponent<BorderStadion>(out BorderStadion borderStadion);
        if (borderStadion)
        {
            isVelositySwapX = false;
            isVelositySwapY = false;
            if (transform.position.x <= pointStadion[0].position.x || transform.position.x >= pointStadion[4].position.x)
            {
                velocity.x *= UnityEngine.Random.Range(-0.8f, -1f);
                isVelositySwapX = true;
            }
            if (transform.position.y <= pointStadion[1].position.y || transform.position.y >= pointStadion[2].position.y)
            {
                velocity.y *= UnityEngine.Random.Range(-0.8f, -1f);
                isVelositySwapY = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent<BorderStadion>(out BorderStadion borderStadion);
        if (borderStadion)
        {

            /*timerOutStadion++;
            if (timerOutStadion> 5)
            {
                if (isVelositySwapX)
                {
                    velocity.y *= -1f;
                }
                if (isVelositySwapY)
                {
                    velocity.x *= -1f;
                }
                timerOutStadion = -5;
            }
            Debug.Log("Bad");*/
        }
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
        float invertVelocity;
        speedPuck = speed;
        target = gate.transform.position;
        velocity = target - transform.position;
        invertVelocity = 3.9f / velocity.x;
        velocity *= invertVelocity;
    }
}
