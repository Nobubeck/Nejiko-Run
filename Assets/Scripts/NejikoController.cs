using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{

    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;

    public int Life()
    {
        return life;
    }

    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //デバック用
        if (Input.GetKeyDown("left")) MoveLeft();
        if (Input.GetKeyDown("right")) MoveRight();
        if (Input.GetKeyDown("space")) Jump();

        if (IsStun())
        {
            //動きを止め気絶状態から復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            //徐々に加速しz方向に常に前進させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            //x方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }


        //重力分の力をマイフレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行 character
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらy方向の速度はリセっとする
        if (controller.isGrounded) moveDirection.y = 0;

        //速度が０以上なら走っているフラグをtrueにする
        animator.SetBool("run", moveDirection.z > 0.0f);
    }

    public void MoveLeft()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }
    public void MoveRight()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump()
    {
        if (IsStun()) return;
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
            //ジャンプトリガーを設定
            animator.SetTrigger("jump");
        }
    }

    //CharacterControllerに衝突判定が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return;

        if (hit.gameObject.tag == "Robo")
        {
            //ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            //ダメージトリガーを設定
            animator.SetTrigger("damage");
            //ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }
}
