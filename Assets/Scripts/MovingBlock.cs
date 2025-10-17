using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [Header("移動距離/時間/間隔")]
    public float moveX = 0.0f;　　//X移動距離
    public float moveY = 0.0f;　　//X移動距離
    public float times = 0.0f;　　//時間
    public float wait = 0.0f;　 　//停止時間

    [Header("乗ってから動くフラグ")]
    public bool isMoveWhenOn = false;  //のった時に動くフラグ

    bool isCanMove = true;
    Vector3 startPos;
    Vector3 endPos;
    bool isReverse = false;
    float movep = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY);
        if (isMoveWhenOn)
        {
            //乗った時最初に動かない
            isCanMove = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanMove)
        {
            float distance = Vector2.Distance(startPos, endPos);  //移動距離
            float ds = distance / times;　　//1秒の移動距離
            float df = ds * Time.deltaTime;　//1フレームの移動距離
            movep += df / distance;　　　　　//移動補完値
            if (isReverse)
            {
                transform.position = Vector2.Lerp(endPos, startPos,movep);　//逆移動
            }
            else
            {
                transform.position = Vector2.Lerp(startPos,endPos, movep);　//正移動
            }
            if (movep >= 1.0f)
            {
                movep = 0.0f;　　　　　　　//移動補完値リセット
                isReverse = !isReverse;　　//移動を逆転
                isCanMove = false;　　　　　//移動停止
                if (isMoveWhenOn == false)
                {
                    Invoke("Move", wait);　//移動フラグをたてる遅延実行
                }
            }
        }
    }

    //移動フラグをたてる
    public void Move()
    {
        isCanMove = true;
    }

    //移動フラグを下ろす
    public void Stop()
    {
        isCanMove = false;
    }

    //接触開始
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //接触したのがプレイヤーばら移動床の子にする
            collision.transform.SetParent(transform);
            if (isMoveWhenOn)
            {
                //乗った時に動くフラグON
                isCanMove = true;
            }
        }
    }

   　//接触終了
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
        
    }

    //移動範囲表示
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        //スプライトのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;
        //初期位置
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        //移動位置
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
