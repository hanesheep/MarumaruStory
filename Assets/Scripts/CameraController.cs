using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    float x, y, z; //�J�����̍��W�����߂邽�߂̕ϐ�

    [Header("�J�����̌��E�l�����߂�ϐ�")]
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;

    [Header("�J�����̃X�N���[���ݒ�")]
    public bool isScrollX; �@�@�@�@�@�@//�������ɋ����X�N���[�����邩�̃t���O
    public float scrollSpeedX = 0.5f;
    public bool isScrollY; �@�@�@�@�@�@//�c�����ɋ����X�N���[�����邩�̃t���O
    public float scrollSpeedY = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Player�^�O���������Q�[���I�u�W�F�N�g��T���ĕϐ�Player�ɑ��
        player = GameObject.FindGameObjectWithTag("Player");
        //�J������Z���W�͏����l�̂܂܂��ێ�������
        z = transform.position.z;
    }


    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            //��������v���C���[��X,Y���W�̈ʒu��ϐ��Ɏ擾
            x = player.transform.position.x;
            y = player.transform.position.y;

            //���������̋����X�N���[���t���O�������Ă�����
            if (isScrollX)
            {
                //�u�O�̍��W�v�Ɂu�ϐ����������Z(���\���ɗR�����Ȃ�)�v�������W
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
            //��茈�߂��e�ϐ�X,Y,Z�̒l���J�����̃|�W�V�����Ƃ���
            transform.position = new Vector3(x, y, z);

        }
    }
}
