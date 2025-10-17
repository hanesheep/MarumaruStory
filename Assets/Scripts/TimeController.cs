using UnityEngine;

public class TimeController : MonoBehaviour
{
    //カウントダウンにするかどうかのフラグ
    //falseならカウントアップ
    public bool isCountDown = true;

    //ゲームの基準となる時間
    public float gameTime = 0;

    //カウントを止めるかどうかのフラグ
    //falseならカウントし続ける、trueならカウント終了
    public bool isTimeOver = false;

    //ユーザーに見せる時間
    public float displayTime = 0;

    //ゲームの経過時間
    float times = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //もしカウントダウンであれば基準時間をユーザーに見えるようにする
        if (isCountDown)
        {
            displayTime = gameTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (isTimeOver == false)
        if(!isTimeOver)
        {
            //停止フラグが立っていないので処理したいがゲームステータスがplayingでなくなった時は止めたい
            if(GameManager.gameState != "playing")
            {
                isTimeOver = true; //停止フラグをON
            }

            //カウントの処理する
            //経過時間の蓄積
            times += Time.deltaTime; //デルタタイムの蓄積

            //カウントダウン形式だった場合
            if (isCountDown)
            {
                //ユーザーに見せたい時間（残時間＝基準時間-経過時間）
                displayTime = gameTime - times;

                if (displayTime <= 0)
                {
                    displayTime = 0; //0という表記に統一
                    isTimeOver=true; //停止フラグをON
                    //GameManager.gameState = "gameover";
                } 
            }
            //カウントアップ形式だった場合
            else
            {
                displayTime = times; //経過時間をユーザーに見せたい時間に代入
                if(displayTime >= gameTime)
                {
                    displayTime = gameTime;
                    isTimeOver = true; //停止フラグをON
                    //GameManager.gameState = "gameover";
                }
            }

            Debug.Log(displayTime);

        }
        
    }
}
