using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private PuckMove puck;
    [SerializeField] private float speedMove;
    [SerializeField] private Vector2 maxLeftTopBorder;
    [SerializeField] private Vector2 maxRightBottomBorder;
    [SerializeField] private Vector3 target;


    private void Start()
    {
        target = puck.transform.position;
    }
    private void Update()
    {
        target = new Vector3(puck.transform.position.x, puck.transform.position.y, transform.position.z);

        //двигаем камеру за шайбой, но если упираемся в край спрайта, камера останавливается
        if (puck.transform.position.x - BorderDisplay.Instance.SizeCamera.x/2 < 0)
            target = new Vector3(0 + BorderDisplay.Instance.SizeCamera.x / 2, target.y, transform.position.z);
        if (puck.transform.position.x + BorderDisplay.Instance.SizeCamera.x / 2 > BorderDisplay.Instance.SizeSpriteBackground.x)
            target = new Vector3(BorderDisplay.Instance.SizeSpriteBackground.x - BorderDisplay.Instance.SizeCamera.x/2, target.y, transform.position.z);
        if (puck.transform.position.y - BorderDisplay.Instance.SizeCamera.y / 2 < 0)
            target = new Vector3(target.x, 0 + BorderDisplay.Instance.SizeCamera.y / 2, transform.position.z);
        if (puck.transform.position.y + BorderDisplay.Instance.SizeCamera.y / 2 > BorderDisplay.Instance.SizeSpriteBackground.y)
            target = new Vector3(target.x, BorderDisplay.Instance.SizeSpriteBackground.y - BorderDisplay.Instance.SizeCamera.y / 2, transform.position.z);

        //двигаем камеру
        transform.position = Vector3.Lerp(transform.position, target, speedMove * Time.deltaTime);


    }
}
