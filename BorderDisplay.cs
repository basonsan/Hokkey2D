using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderDisplay : MonoBehaviour
{
    public static BorderDisplay Instance;
    [SerializeField] private Vector2 screenResolution;
    [SerializeField] private Camera cameraScene;
    [SerializeField] private SpriteRenderer spriteStadion;

    private Vector2 sizeCamera;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (Application.isEditor == false && Application.isPlaying == true)
        {
            screenResolution.x = Screen.width;
            screenResolution.y = Screen.height;
        }
        //ширина и высота камеры
        sizeCamera = RightTopPosition - LeftBottomPosition;
    }

    //левая позиция камеры
    public Vector2 LeftBottomPosition
    {
        get
        {
            return cameraScene.ScreenToWorldPoint(new Vector3(0, 0, 0));
        }
    }

    //правая позиция камеры
    public Vector2 RightTopPosition
    {
        get
        {
            return cameraScene.ScreenToWorldPoint(new Vector3(screenResolution.x, screenResolution.y, 0));
        }
    }

    public Vector2 SizeCamera
    {
        get
        {
            return sizeCamera;
        }
    }

    //правая координата спрайта
    public Vector2 SizeSpriteBackground
    {
        get
        {
            return spriteStadion.size;
        }
    }
}
