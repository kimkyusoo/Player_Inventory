using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove3D : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float baseSpeed = 5.0f;
    public float runSpeed = 10.0f;

    private Rigidbody rb;
    private Animator anim;

    public InputAction MoveAction;
    public InputAction RunAction;
    Vector2 inputDirection;

    public Direction currentDirection;

    public TextMeshProUGUI speedText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        speedText.text = $"Run Speed: {runSpeed.ToString()}";
    }

    private void OnEnable() { 
        MoveAction.Enable(); RunAction.Enable();
        PlayerInventory.effectApplied += EnforcSpeed;
    }
    private void OnDisable() 
    { 
        MoveAction.Disable(); RunAction.Disable();
        PlayerInventory.effectApplied -= EnforcSpeed;
    }

    private void Update()
    {
        moveSpeed = RunAction.IsPressed() ? runSpeed : baseSpeed;
        inputDirection = MoveAction.ReadValue<Vector2>();

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (anim != null)
        {
            float horizontalSpeed = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
            anim.SetFloat("Speed", horizontalSpeed);
        }

        UpdateDirection();
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpeed;
        rb.linearVelocity = new Vector3(moveVec.x, rb.linearVelocity.y, moveVec.z);
    }

    private void UpdateDirection()
    {
        if (inputDirection.sqrMagnitude < 0.01f)
        {
            currentDirection = Direction.None;
            return;
        }

        float x = inputDirection.x;
        float y = inputDirection.y;

        if (x > 0.1f) 
        {
            if (y > 0.1f) currentDirection = Direction.RightUp;
            else if (y < -0.1f) currentDirection = Direction.RightDown;
            else currentDirection = Direction.Right;
        }
        else if (x < -0.1f) 
        {
            if (y > 0.1f) currentDirection = Direction.LeftUp;
            else if (y < -0.1f) currentDirection = Direction.LeftDown;
            else currentDirection = Direction.Left;
        }
        {
            if (y > 0.1f) currentDirection = Direction.Up;
            else if (y < -0.1f) currentDirection = Direction.Down;
        }
    }

    private void EnforcSpeed(ItemEffect effect)
    {
        if(effect == ItemEffect.SpeedUp)
        {
            runSpeed += 5;
            speedText.text = $"Run Speed: {runSpeed.ToString()}";
        }
    }
}