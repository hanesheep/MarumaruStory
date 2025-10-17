using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̔\�͒l")]
    public float speed = 3.0f;
    public float jumpPower = 9.0f;

    [Header("�n�ʔ���̑Ώۃ��C���[")]
    public LayerMask groundLayer;

    Rigidbody2D rbody; //Player�ɂ��Ă���RigidBody2D���������߂̕ϐ�
    Animator animator; //Animator�R���|�[�l���g���������߂̕ϐ�

    float axisH; //���͂̕������L�����邽�߂̕ϐ�
 
    bool gojump = false; //���W�����v�t���O(�����l�͋U=false)
 
    bool onGround = false; //�n�ʂɂ��邩�ǂ����̔���i���遁true�j

    AudioSource audio;
    public AudioClip se_Jump;
    public AudioClip se_ItemGet;
    public AudioClip se_Damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Player�ɂ��Ă���R���|�[�l���g�����擾

        animator = GetComponent<Animator>(); //Animator�ɂ��Ă���R���|�[�l���g�����擾

        audio =GetComponent<AudioSource>();  //AudioSource�R���|�[�l���g�̏�����
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[���̃X�e�[�^�X��playing�łȂ��Ȃ�B
        if (GameManager.gameState != "playing")
        {
            return; //����1�t���������I��
        }
        ////���������������̃L�[�������ꂽ��(�����Ȃ��Ă������͂���)
        //if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //Verocity�̌��ƂȂ�l�̎擾�i1.0���������j
            axisH = Input.GetAxisRaw("Horizontal");

            if(axisH > 0)
            {
                //�E������
                transform.localScale = new Vector3(1,1,1);
            }
            else if(axisH < 0)
            {
                //��������
                transform.localScale = new Vector3(-1, 1, 1);
            }

            //GetButtonDown���\�b�h�������Ɏw�肵���{�^���������ꂽ��True�i�������l�̋t�j�ɂȂ�
            if (Input.GetButtonDown("Jump"))
            {
                Jump(); //Jump���\�b�h�̔���
            }
        }

        

    }

    //1�b�Ԃ�50��(50fps)�J��Ԃ��悤�ɐ��䂵�Ȃ���s���J��Ԃ����\�b�h
    void FixedUpdate()
    {
        if (GameManager.gameState != "playing")
        {
            return; //����1�t���������I��
        }
        //�n�ʔ�����T�[�N���L���X�g�ōs���Ă��̌��ʂ�ϐ�onGround�ɑ��
        onGround = Physics2D.CircleCast(
            transform.position,   //���ˈʒu���v���C���[�̈ʒu�i��_�j
            0.2f,                 //�T�[�N���L���X�g�̉~�̑傫���i���a�j
            new Vector2(0,1.0f),�@//���˕�����������
            0,                     //���ˋ���
            groundLayer       //�ΏۂƂȂ郌�C���[��� ��LayerMask�^
            );

        //velocity�ɒl����
        rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);

        //�W�����v�t���O����������
        if (gojump)
        {
            //�W�����v�����遨�v���C���[����ɉ����o��
            rbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            gojump = false;
        }

        //if (onGround) //�n�ʂ̏�ɂ���Ƃ�
        //{
            if (axisH == 0) //���E��������Ă��Ȃ�
            {
                animator.SetBool("Run", false);�@//Idle�A�j���ɐ؂�ւ�
            }
            else //���E��������Ă���
            {
                animator.SetBool("Run", true);�@//Run�A�j���ɐ؂�ւ�
            }
        //}
    }

    //Jump��p�̃��\�b�h
    void Jump()
    {
        if (onGround)
        {
            //SE��炷
            audio.PlayOneShot(se_Jump);

            gojump = true; //Jump�t���O��ON�ɂ���
            animator.SetTrigger("Jump");
        }
        
    }

    //isTrigger�����������Ă���Collider�ƂԂ������珈�������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�Ԃ��������肪"Goal"�^�O�������Ă�����
        if (collision.gameObject.CompareTag("Goal"))
        {
            GameManager.gameState = "gameclear";
            Debug.Log("�S�[���ɐڐG�����I");
            Goal();
        }

        //�Ԃ��������肪"Dead"�^�O�������Ă�����i����j
        if (collision.gameObject.CompareTag("Dead"))
        {
            //SE��炷
            audio.PlayOneShot(se_Damage);

            GameManager.gameState = "gameover";
            Debug.Log("�s�k��");
            GameOver();
        }
        //�A�C�e���ɐG�ꂽ��X�e�[�W�X�R�A�ɉ��Z
        if (collision.gameObject.CompareTag("ItemScore"))
        {
            //SE��炷
            audio.PlayOneShot(se_ItemGet);

            GameManager.stageScore += collision.gameObject.GetComponent<ItemData>().value;
            Destroy(collision.gameObject);
        }
    }

    //�S�[�����\�b�h
    public void Goal()
    {
        animator.SetBool("Clear",true); //�N���A�A�j���ɐ؂�ւ�
        GameStop();                      //�v���C���[��Velocity���~�߂郁�\�b�h
    }

    //�Q�[���I�[�o�[���\�b�h
    public void GameOver()
    {
        animator.SetBool("Dead", true); //�f�b�h�A�j���ɐ؂�ւ�
        GameStop();

        //�����蔻��𖳌��ɂ���
        GetComponent<CapsuleCollider2D>().enabled = false;

        //������ɔ�ђ��˂�����
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        //�v���C���[�����ԍ�(��(��3.0��)�b��)�Ŗ���
        Destroy(gameObject,3.0f);
    }

    void GameStop()
    {
        //���x��0�Ƀ��Z�b�g
        //rbody.linearVelocity = new Vector2(0, 0);
        rbody.linearVelocity = Vector2.zero;
    }
}
