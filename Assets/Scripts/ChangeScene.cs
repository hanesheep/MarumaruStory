using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName; //切り替えたいScene名を指名
    public bool toTitle;     //タイトルへの切り替えかどうかのフラグ

    //シーンを切り替える機能を持ったメソッド作成
    public void Load()
    {
        //シーンが切り替わる際はいずれにしてもステージスコアはリセット
        GameManager.stageScore = 0;

        //toTitleフラグがTrueになっている場合はタイトルに戻ることが予想されるのでトータルスコアもリセット
        if (toTitle) GameManager.totalScore = 0;


        //引数に指定した名前のシーンに切り替えしてくれるメソッドの呼び出し
        SceneManager.LoadScene(sceneName);
    }

}
