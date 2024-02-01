
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Настройка референсов")] [SerializeField]
    private Transform cameraTransform;

    [SerializeField] private Camera cameraComponent;

    [Space] [Header("Настройка свойств")] [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField] private float sensitivity = 5f;

    [SerializeField] private bool boundedView;

    [Tooltip("Только для переспективной проекции. Добавляет границы для осмотра")] [SerializeField]
    private float viewingBoundaries = 5f;

    [SerializeField] private Vector2 turn;
    private bool fixedCamera = true;


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            fixedCamera = !fixedCamera;
    }

    private void FixedUpdate()
    {
        if (cameraComponent.orthographic)
            MoveOrthographic();
        else
        {
            if (boundedView)
                MovePerspectiveInBounded();
            else
                MovePerspective();
        }
    }

    private void MoveOrthographic()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        var forwardProjectionOnXZ = cameraTransform.forward;
        forwardProjectionOnXZ.y = 0;
        var direction = (forwardProjectionOnXZ * moveY + cameraTransform.right * moveX) * (moveSpeed * Time.fixedDeltaTime);
        cameraTransform.Translate(direction, Space.World);

        if (fixedCamera) return;
        float rotationX = Input.GetAxisRaw("Mouse X") * sensitivity;
        cameraTransform.Rotate(0, rotationX, 0, Space.World);
    }

    private void MovePerspective()
    {
        var direction = (Input.GetAxis("Horizontal") * cameraTransform.right +
                         Input.GetAxis("Vertical") * cameraTransform.forward) *
                        (moveSpeed * Time.fixedDeltaTime);
        cameraTransform.Translate(direction, Space.World);

        if (fixedCamera) return;
        turn.x += Input.GetAxisRaw("Mouse X") * sensitivity;
        turn.y += Input.GetAxisRaw("Mouse Y") * sensitivity;
        //print($"{Input.GetAxisRaw("Mouse X")} - {Input.GetAxisRaw("Mouse Y")}");
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }

    private void MovePerspectiveInBounded()
    {
        var direction = (Input.GetAxis("Horizontal") * cameraTransform.right +
                         Input.GetAxis("Vertical") * cameraTransform.forward) *
                        (moveSpeed * Time.fixedDeltaTime);

        cameraTransform.Translate(direction, Space.World);

        if (fixedCamera) return;
        turn.x = (Input.mousePosition.x / cameraComponent.pixelWidth - 0.5f) * 2 * viewingBoundaries;
        turn.y = (Input.mousePosition.y / cameraComponent.pixelHeight - 0.5f) * 2 * viewingBoundaries;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}