using UnityEngine;

// Moves the player, turns toward the mouse, and provides a short evasive dash.
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f;
    public float rotateSpeed = 1800f;
    public bool rotateToMouse = true;

    public KeyCode dashKey = KeyCode.LeftShift;
    public float dashDistance = 4.5f;
    public float dashDuration = 0.16f;
    public float dashCooldown = 1.8f;

    public bool IsDashing { get; private set; }

    private Animator playerAnimator;
    private Camera mainCamera;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private bool dashRequested;
    private float dashEndTime;
    private float nextDashTime;
    private Vector3 dashDirection;
    private GUIStyle dashStyle;

    private void Start() {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetKeyDown(dashKey))
        {
            dashRequested = true;
        }
    }

    private void OnDisable() {
        IsDashing = false;
        dashRequested = false;
    }

    private void FixedUpdate() {
        Rotate();

        if (dashRequested)
        {
            TryStartDash();
            dashRequested = false;
        }

        Move();
        playerAnimator.SetFloat("Move", IsDashing ? 1f : GetMoveDirection().magnitude);
    }

    private void Move() {
        if (IsDashing)
        {
            if (Time.time >= dashEndTime)
            {
                IsDashing = false;
            }
            else
            {
                float dashSpeed = dashDistance / dashDuration;
                Vector3 dashMoveDistance = dashDirection * dashSpeed * Time.deltaTime;
                playerRigidbody.MovePosition(playerRigidbody.position + dashMoveDistance);
                return;
            }
        }

        Vector3 moveDistance = GetMoveDirection() * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void TryStartDash() {
        if (IsDashing || Time.time < nextDashTime)
        {
            return;
        }

        dashDirection = GetMoveDirection();
        if (dashDirection.sqrMagnitude <= 0.01f)
        {
            dashDirection = transform.forward;
        }

        dashDirection.y = 0f;
        dashDirection.Normalize();

        IsDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;
    }

    private Vector3 GetMoveDirection() {
        Vector3 inputDirection = new Vector3(playerInput.rotate, 0f, playerInput.move);
        return Vector3.ClampMagnitude(inputDirection, 1f);
    }

    private void Rotate() {
        if (rotateToMouse && TryGetMouseLookPoint(out Vector3 lookPoint))
        {
            Vector3 lookDirection = lookPoint - playerRigidbody.position;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                Quaternion nextRotation = Quaternion.RotateTowards(
                    playerRigidbody.rotation,
                    targetRotation,
                    rotateSpeed * Time.deltaTime);

                playerRigidbody.MoveRotation(nextRotation);
            }
        }
    }

    private bool TryGetMouseLookPoint(out Vector3 lookPoint) {
        lookPoint = Vector3.zero;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            return false;
        }

        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, playerRigidbody.position);

        if (playerPlane.Raycast(mouseRay, out float distance))
        {
            lookPoint = mouseRay.GetPoint(distance);
            return true;
        }

        return false;
    }

    private void OnGUI() {
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        EnsureDashStyle();

        float cooldownRemain = Mathf.Max(0f, nextDashTime - Time.time);
        string dashText = cooldownRemain <= 0f
            ? "Dash Ready : Shift"
            : "Dash Cooldown : " + cooldownRemain.ToString("0.0") + "s";

        GUI.Label(new Rect(18f, Screen.height - 44f, 220f, 28f), dashText, dashStyle);
    }

    private void EnsureDashStyle() {
        if (dashStyle != null)
        {
            return;
        }

        dashStyle = new GUIStyle(GUI.skin.label);
        dashStyle.fontSize = 18;
        dashStyle.fontStyle = FontStyle.Bold;
        dashStyle.normal.textColor = new Color(0.35f, 0.95f, 1f);
    }
}
