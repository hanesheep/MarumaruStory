using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの能力値")]
    public float speed = 3.0f;
    public float jumpPower = 9.0f;

    [Header("地面判定の対象レイヤー")]
    public LayerMask groundLayer;

    Rigidbody2D rbody; //PlayerについているRigidBody2Dを扱うための変数
    Animator animator; //Animatorコンポーネントを扱うための変数

    float axisH; //入力の方向を記憶するための変数
 
    bool gojump = false; //←ジャンプフラグ(初期値は偽=false)
 
    bool onGround = false; //地面にいるかどうかの判定（いる＝true）

    AudioSource audio;
    public AudioClip se_Jump;
    public AudioClip se_ItemGet;
    public AudioClip se_Damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Playerについているコンポーネント情報を取得

        animator = GetComponent<Animator>(); //Animatorについているコンポーネント情報を取得

        audio =GetComponent<AudioSource>();  //AudioSourceコンポーネントの情報を代入
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームのステータスがplayingでないなら。
        if (GameManager.gameState != "playing")
        {
            return; //その1フレを強制終了
        }
        ////もしも水平方向のキーが押されたら(書かなくても成立はする)
        //if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //Verocityの元となる値の取得（1.0ｆずつ動く）
            axisH = Input.GetAxisRaw("Horizontal");

            if(axisH > 0)
            {
                //右を向く
                transform.localScale = new Vector3(1,1,1);
            }
            else if(axisH < 0)
            {
                //左を向く
                transform.localScale = new Vector3(-1, 1, 1);
            }

            //GetButtonDownメソッド→引数に指定したボタンが押されたらTrue（＝初期値の逆）になる
            if (Input.GetButtonDown("Jump"))
            {
                Jump(); //Jumpメソッドの発動
            }
        }

        

    }

    //1秒間に50回(50fps)繰り返すように制御しながら行う繰り返しメソッド
    void FixedUpdate()
    {
        if (GameManager.gameState != "playing")
        {
            return; //その1フレを強制終了
        }
        //地面判定をサークルキャストで行ってその結果を変数onGroundに代入
        onGround = Physics2D.CircleCast(
            transform.position,   //発射位置＝プレイヤーの位置（基準点）
            0.2f,                 //サークルキャストの円の大きさ（半径）
            new Vector2(0,1.0f),　//発射方向※下方向
            0,                     //発射距離
            groundLayer       //対象となるレイヤー情報 ※LayerMask型
            );

        //velocityに値を代入
        rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);

        //ジャンプフラグがたったら
        if (gojump)
        {
            //ジャンプさせる→プレイヤーを上に押し出す
            rbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            gojump = false;
        }

        //if (onGround) //地面の上にいるとき
        //{
            if (axisH == 0) //左右が押されていない
            {
                animator.SetBool("Run", false);　//Idleアニメに切り替え
            }
            else //左右が押されている
            {
                animator.SetBool("Run", true);　//Runアニメに切り替え
            }
        //}
    }

    //Jump専用のメソッド
    void Jump()
    {
        if (onGround)
        {
            //SEを鳴らす
            audio.PlayOneShot(se_Jump);

            gojump = true; //JumpフラグをONにする
            animator.SetTrigger("Jump");
        }
        
    }

    //isTrigger特性を持っているColliderとぶつかったら処理される
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ぶつかった相手が"Goal"タグを持っていたら
        if (collision.gameObject.CompareTag("Goal"))
        {
            GameManager.gameState = "gameclear";
            Debug.Log("ゴールに接触した！");
            Goal();
        }

        //ぶつかった相手が"Dead"タグを持っていたら（同上）
        if (collision.gameObject.CompareTag("Dead"))
        {
            //SEを鳴らす
            audio.PlayOneShot(se_Damage);

            GameManager.gameState = "gameover";
            Debug.Log("敗北者");
            GameOver();
        }
        //アイテムに触れたらステージスコアに加算
        if (collision.gameObject.CompareTag("ItemScore"))
        {
            //SEを鳴らす
            audio.PlayOneShot(se_ItemGet);

            GameManager.stageScore += collision.gameObject.GetComponent<ItemData>().value;
            Destroy(collision.gameObject);
        }
    }

    //ゴールメソッド
    public void Goal()
    {
        animator.SetBool("Clear",true); //クリアアニメに切り替え
        GameStop();                      //プレイヤーのVelocityを止めるメソッド
    }

    //ゲームオーバーメソッド
    public void GameOver()
    {
        animator.SetBool("Dead", true); //デッドアニメに切り替え
        GameStop();

        //当たり判定を無効にする
        GetComponent<CapsuleCollider2D>().enabled = false;

        //少し上に飛び跳ねさせる
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        //プレイヤーを時間差(ｘ(＝3.0ｆ)秒後)で抹消
        Destroy(gameObject,3.0f);
    }

    void GameStop()
    {
        //速度を0にリセット
        //rbody.linearVelocity = new Vector2(0, 0);
        rbody.linearVelocity = Vector2.zero;
    }
}
