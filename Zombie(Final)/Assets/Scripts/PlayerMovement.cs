using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 움직임의 속도
    public float rotateSpeed = 720f; // 마우스 방향을 향해 회전하는 속도
    public bool rotateToMouse = true; // 마우스 위치 기반 회전 사용 여부

    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private Camera mainCamera; // 마우스 위치를 월드 좌표로 바꾸기 위한 카메라
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 회전 실행
        Rotate();
        // 움직임 실행
        Move();

        // 입력값에 따라 애니메이터의 Move 파라미터 값을 변경
        playerAnimator.SetFloat("Move", GetMoveDirection().magnitude);
    }

    // 캐릭터가 바라보는 방향과 상관없이 월드 좌표 기준으로 움직임
    private void Move() {
        Vector3 moveDistance = GetMoveDirection() * moveSpeed * Time.deltaTime;

        // 리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private Vector3 GetMoveDirection() {
        Vector3 inputDirection = new Vector3(playerInput.rotate, 0f, playerInput.move);
        return Vector3.ClampMagnitude(inputDirection, 1f);
    }

    // 마우스 위치를 바라보도록 캐릭터를 회전
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
                return;
            }
        }

        // 카메라가 없거나 마우스 위치 계산에 실패하면 현재 방향을 유지
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
}
