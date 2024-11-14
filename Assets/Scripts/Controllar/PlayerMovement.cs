using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speedOffset = 0.5f;

    private Vector2 m_beginPos;
    private Vector2 m_endPos;
    private float rotateAngle;

    private CharacterController charCtrl;

    private float speed;
    private Vector3 moveDirection;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_beginPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            m_endPos = Input.mousePosition;
            speed = ( m_beginPos - m_endPos ).magnitude;

            rotateAngle = Mathf.Atan2((m_endPos.y - m_beginPos.y), (m_endPos.x - m_beginPos.x)) * Mathf.Rad2Deg - 90f;

            if (rotateAngle <= 180f)
                rotateAngle -= 360f;

            transform.eulerAngles = new Vector3(0f, -rotateAngle, 0f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            speed = 0f;
        }

        moveDirection = transform.forward * speed * Time.deltaTime * speedOffset;

        charCtrl.Move(moveDirection);

        animator.SetFloat("RunSpeed", speed * Time.deltaTime );
    }
}
