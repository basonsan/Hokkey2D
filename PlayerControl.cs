using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    [Header("Настройки персонажа")]

    [Tooltip("Скорость персонажа при столкновении")] [Range(0.5f, 1f)] [SerializeField] private float speedInCollision;
    [Tooltip("Дефолтная скорость персонажа")] [Range(1f, 3f)] [SerializeField] private float minSpeed;
    [Tooltip("Максимальная скорость персонажа")] [Range(1f, 5f)] [SerializeField] private float maxSpeed;
    [Tooltip("Ускорение персонажа")] [Range(1f, 3f)] [SerializeField] private float boostSpeed;
    [Tooltip("Сила удара при привышении лимита")] [Range(0.5f, 1f)] [SerializeField] private float errorStrongStrike;
    [Tooltip("Минимальная сила удара по шайбе")] [Range(1f, 3f)] [SerializeField] private float minStrongStrike;
    [Tooltip("Максимальная сила удара по шайбе")] [Range(1f, 5f)] [SerializeField] private float maxStrongStrike;
    [Tooltip("Ускорение силы удара по шайбе")] [Range(1f, 3f)] [SerializeField] private float bostStrongStrike;
    [Tooltip("Время восстановления после столкновения")] [Range(1f, 3f)] [SerializeField] private float maxtimerCrash;

    [Header("Вспомогательные элементы")]

    [Tooltip("Точка прилипания шайбы")] [SerializeField] private Transform puckPoint;
    [Tooltip("Префаб шайбы")] [SerializeField] private PuckMove puck;
    [Tooltip("Ворота")] [SerializeField] private Gate gate;
    [Tooltip("Бокс колайдер Клюшки")] [SerializeField] private BoxCollider2D stickTriger;

    [Header("Debug - Не заполнять")]
    [SerializeField] private float speed;
    [SerializeField] private float strongStrike;
    [SerializeField] private int CountTrigerBorderGateOn;
    [SerializeField] private bool isPuckOnMe;
    [SerializeField] private bool isRightDirection = true;
    [SerializeField] private bool isCrash = false;
    [SerializeField] private float timerCrash;
    [SerializeField] private Vector3 target;
    private UIPlayer uiPlayer;
    public event Action<float, Gate> _StrikePuck;

    public Transform PuckPoint => puckPoint;
    private void Awake()
    {
        speed = minSpeed;
        CountTrigerBorderGateOn = 0;
        puck.OntrigerPlaer += CheckPlayer;
        strongStrike = minStrongStrike;
        isPuckOnMe = false;
        gameObject.TryGetComponent<UIPlayer>(out uiPlayer);
    }

    private void CheckPlayer(PlayerControl player)
    {
        if (gameObject == player.gameObject)
        {
            //Debug.Log("itsMe");
            isPuckOnMe = true;
        } else
        {
            //Debug.Log("NoMe");
        }
    }

    void Update()
    {
        if (isPuckOnMe)
        {
            
            Debug.Log(strongStrike);
            if (Input.GetMouseButtonUp(0))
            {
                if (strongStrike != minStrongStrike)
                {
                    StrikePuck();
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                strongStrike += Time.deltaTime;
            }
            if (strongStrike > minStrongStrike)
            {
                uiPlayer.SetStrongPuck((strongStrike - minStrongStrike) / (maxStrongStrike - minStrongStrike));
                strongStrike += Time.deltaTime * bostStrongStrike;
                if (strongStrike > maxStrongStrike)
                {
                    strongStrike = errorStrongStrike;
                    isCrash = true;
                    timerCrash = maxtimerCrash;
                    StrikePuck();
                }
                return;
            }
        }
        if (!isCrash)
        {
            RotationPlayer();
            Move();
        } else
        {
            CrashMove();
        }
    }

    private void StrikePuck()
    {
        _StrikePuck.Invoke(strongStrike, gate);
        strongStrike = minStrongStrike;
        stickTriger.enabled = false;
        isPuckOnMe = false;
        uiPlayer.SetStrongPuck(0f);
        return;
    }

    private void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (stickTriger.enabled == false)
            {
                stickTriger.enabled = true;
            }
        }
        target.x = transform.position.x + Input.GetAxis("Horizontal");
        target.y = transform.position.y + Input.GetAxis("Vertical");
        target.z = transform.position.z;
        if (CountTrigerBorderGateOn == 0)
        {
            if (Input.GetAxis("Horizontal") == 0)
                speed = minSpeed;
            if (Input.GetAxis("Horizontal") == 1 && speed < maxSpeed)
                speed += Time.deltaTime * boostSpeed;
            if (Input.GetAxis("Horizontal") == -1 && speed < maxSpeed)
                speed += Time.deltaTime * boostSpeed;
        }
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }

    private void RotationPlayer()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 180, 0)), 1000);
            isRightDirection = true;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 1000);
            isRightDirection = false;
        }
    }

    private void SlowSpeed()
    {
        CountTrigerBorderGateOn += 1;
        speed = speedInCollision;
    }
    private void NormalizeSpeed()
    {
        CountTrigerBorderGateOn -= 1;
        if (CountTrigerBorderGateOn == 0)
        {
            speed = minSpeed;
        }
    }

    private void CrashMove()
    {
        timerCrash -= Time.deltaTime;
        if (timerCrash > 0)
        {
            //замедляем персонажа 0.1f
            transform.position = Vector3.Lerp(transform.position, target, speed * 0.1f * Time.deltaTime);
        } else
        {
            isCrash = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent<BorderStadion>(out BorderStadion borderStadion);
        collision.transform.root.TryGetComponent<Gate>(out Gate gate);
        if (borderStadion || gate)
        {
            SlowSpeed();
        }
        if (gate)
        {
            isCrash = true;
            timerCrash = maxtimerCrash;
            target.x = transform.position.x - Input.GetAxis("Horizontal");
            target.y = transform.position.y - Input.GetAxis("Vertical");
            target.z = transform.position.z;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent<BorderStadion>(out BorderStadion borderStadion);
        collision.transform.root.TryGetComponent<Gate>(out Gate gate);
        if (borderStadion || gate)
        {
            NormalizeSpeed();
        }
    }
}
