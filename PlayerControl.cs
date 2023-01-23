using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float maxBoostSpeed;
    [SerializeField] Transform centerStadion;
    private Vector3 target;
    private float boostSpeed;
    private bool isTrigerOn;

    private void Awake()
    {
        boostSpeed = 1f;
        isTrigerOn = false;
    }

    void Update()
    {
        
        target.x = transform.position.x + Input.GetAxis("Horizontal");
        target.y = transform.position.y + Input.GetAxis("Vertical");
        target.z = transform.position.z;
        if (Input.GetAxis("Horizontal") == 0)
            boostSpeed = 1f;
        if (Input.GetAxis("Horizontal") == 1 && boostSpeed < maxBoostSpeed)
            boostSpeed += Time.deltaTime;
        if (Input.GetAxis("Horizontal") == -1 && boostSpeed < maxBoostSpeed)
            boostSpeed += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target, speed * boostSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTrigerOn = true;
        while (isTrigerOn)
        {
            transform.position = Vector3.Lerp(transform.position, centerStadion.position, Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTrigerOn = false;
    }




}
