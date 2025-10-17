using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    float x, y, z; //カメラの座標を決めるための変数

    [Header("カメラの限界値を決める変数")]
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;

    [Header("カメラのスクロール設定")]
    public bool isScrollX; 　　　　　　//横方向に強制スクロールするかのフラグ
    public float scrollSpeedX = 0.5f;
    public bool isScrollY; 　　　　　　//縦方向に強制スクロールするかのフラグ
    public float scrollSpeedY = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Playerタグをもったゲームオブジェクトを探して変数Playerに代入
        player = GameObject.FindGameObjectWithTag("Player");
        //カメラのZ座標は初期値のままを維持したい
        z = transform.position.z;
    }


    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            //いったんプレイヤーのX,Y座標の位置を変数に取得
            x = player.transform.position.x;
            y = player.transform.position.y;

            //もしも横の強制スクロールフラグが立っていたら
            if (isScrollX)
            {
                //「前の座標」に「変数分だけ加算(性能差に由来しない)」した座標
                x = transform.position.x + (scrollSpeedX * Time.deltaTime);
            }

            if (x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }

            if (isScrollY)
            {
                y = transform.position.y + (scrollSpeedY * Time.deltaTime);
            }

            if (y < topLimit)
            {
                y = topLimit;
            }
            else if (y > bottomLimit)
            {
                y = bottomLimit;
            }
            //取り決めた各変数X,Y,Zの値をカメラのポジションとする
            transform.position = new Vector3(x, y, z);

        }
    }
}
