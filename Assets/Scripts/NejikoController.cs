using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;
    public float speedJump;
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
        //接地しているかどうかの判定
        if(controller.isGround)
        {
            if(Input.GetAxis("Vertical") > 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }
        //方向の回転
            transition.Rotate(0,Input.GetAxis("Horizontal") * 3,0);
        //ジャンプ処理
            if(Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;
                animator.SetTrigger("jump");
            }
        }

        //重力分の力をマイフレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行 character
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらy方向の速度はリセっとする
        if(controller.isGround) moveDirection.y = 0;

        //速度が０以上なら走っているフラグをtrueにする
        aninmator.SetBool("run",moveDirection.z > 0.0f);
    }
}
