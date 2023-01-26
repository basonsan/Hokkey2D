using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxBoostSpeed;
    [SerializeField] private Transform puckPoint;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PuckMove puck;
    [SerializeField] private float minStrongStrike;
    [SerializeField] private float maxStrongStrike;
    [SerializeField] private Gate gate;
    [SerializeField] private BoxCollider2D bcTriger;
    [SerializeField] private BoxCollider2D bcView;
    private Vector3 target;
    private float boostSpeed;
    private float speed;
    private bool isTrigerBorderGateOn;
    private float strongStrike;
    private PlayerControl thisPlayer;

    public event Action<float, Gate> StrikePuck;

    public Transform PuckPoint => puckPoint;
    private void Awake()
    {
        speed = maxSpeed;
        boostSpeed = 1f;
        isTrigerBorderGateOn = false;
        puck.OntrigerPlaer += CheckPlayer;
        strongStrike = minStrongStrike;
    }

    private void CheckPlayer(PlayerControl player)
    {
        Debug.Log(player);
        if (gameObject == player.gameObject)
        {
            Debug.Log("itsMe");
        } else
        {
            Debug.Log("NoMe");
        }
    }

    void Update()
    {
        Move();
        if(Input.GetMouseButtonDown(0))
        {
            strongStrike += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            StrikePuck.Invoke(strongStrike, gate);
            strongStrike = minStrongStrike;
            bcTriger.enabled = false;
            bcView.enabled = false;
        }
    }

    private void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (bcTriger.enabled == false)
            {
                bcTriger.enabled = true;
                bcView.enabled = true;
            }
        }
        target.x = transform.position.x + Input.GetAxis("Horizontal");
        target.y = transform.position.y + Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") < 0)
        {
            if (!sprite.flipX)
            {
                sprite.flipX = true;
                puckPoint.transform.localPosition *= -1;
            }
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (sprite.flipX)
            {
                sprite.flipX = false;
                puckPoint.transform.localPosition *= -1;
            }
        }
        target.z = transform.position.z;
        if (Input.GetAxis("Horizontal") == 0)
            boostSpeed = 1f;
        if (isTrigerBorderGateOn == false)
        {
            if (Input.GetAxis("Horizontal") == 1 && boostSpeed < maxBoostSpeed)
                boostSpeed += Time.deltaTime;
            if (Input.GetAxis("Horizontal") == -1 && boostSpeed < maxBoostSpeed)
                boostSpeed += Time.deltaTime;
        }
        transform.position = Vector3.Lerp(transform.position, target, speed * boostSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent<BorderStadion>(out BorderStadion borderStadion);
        collision.transform.root.TryGetComponent<Gate>(out Gate gate);
        if (borderStadion || gate)
        {
            isTrigerBorderGateOn = true;
            boostSpeed = 1f;
            speed = 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent<BorderStadion>(out BorderStadion borderStadion);
        collision.transform.root.TryGetComponent<Gate>(out Gate gate);
        if (borderStadion || gate)
        {
            isTrigerBorderGateOn = false;
            speed = maxSpeed;
        }
    }
}
