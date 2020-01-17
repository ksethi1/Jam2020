using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(ICharacterInput))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float responseSpeed=3;
    Animator animator;
    SpriteRenderer bodySprite;
    ICharacterInput input;
    Rigidbody2D rigid;
    Vector3 movementDirection;
    Vector3 targetVelocity;
    bool canMove=true;
    bool isDashing=false;

    readonly int walkBoolHash = Animator.StringToHash("Walking");

    private void Start()
    {
        input = GetComponent<ICharacterInput>();
        bodySprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid= GetComponent<Rigidbody2D>();
        movementDirection.z = 0;
    }

    

    private void LateUpdate()
    {
        if(canMove)
            targetVelocity = movementDirection * movementSpeed;
        rigid.velocity = Vector2.Lerp(rigid.velocity, targetVelocity, Time.deltaTime * responseSpeed);
    }

    private void Update()
    {
        if (canMove)
            TakeMoveInput();
        else
            movementDirection = Vector3.zero;

        Flip();
        if(animator)
            SetAnimations();
    }

    void Flip()
    {
        if (input.MouseXDirection >= 0)
            bodySprite.flipX = false;
        else
            bodySprite.flipX = true;
    }
    void SetAnimations()
    {
        if (movementDirection.x != 0 || movementDirection.y != 0)
            animator?.SetBool(walkBoolHash, true);
        else
            animator?.SetBool(walkBoolHash, false);
    }
    void TakeMoveInput()
    {
        input.SetMovementInputs();
        movementDirection.x = input.HorizontalInput;
        movementDirection.y = input.VerticalInput;
        movementDirection=movementDirection.normalized;
    }

    public void AddSpeed(float value)
    {
        movementSpeed +=value;
    }

    public void DisableMovement()
    {
        canMove= false;
        targetVelocity = Vector3.zero;
        if(rigid!=null)
            rigid.velocity = Vector3.zero;
    }
    public void DisableMovement(Vector2 targetVelocity)
    {
        DisableMovement();
        this.targetVelocity = targetVelocity;
        rigid.velocity = targetVelocity;
    }
    public void EnableMovement()
    {
        canMove= true;
    }
}
