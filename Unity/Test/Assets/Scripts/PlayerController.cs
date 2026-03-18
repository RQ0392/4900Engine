using UnityEngine;
using UnityEngine.InputSystem;

namespace AOTADev
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private InputSystem_Actions _inputActions;
        private int _attackCount = 0; // Extra Challenge Counter

        [Header("Camera")]
        [SerializeField] private Camera _camera;
        public Camera MainCamera { get => _camera; set => _camera = value; }

        // Increase these if look is too slow
        [SerializeField] private float sensitivityX = 0.5f;
        [SerializeField] private float sensitivityY = 0.5f;
        [SerializeField] private float minPitch = -89f;
        [SerializeField] private float maxPitch = 89f;
        [SerializeField] private bool lockCursor = true;

        private float _pitch;

        [Header("Movement")]
        [SerializeField] private float _gravityAccel = -20f;
        [SerializeField] private float acceleration = 40f;
        [SerializeField] private float maxMoveSpeed = 6f;
        [SerializeField] private float brakingFactor = 16f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float groundCheckDistance = 1.6f;

        [Header("Grounding")]
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private int ignoreLayer = 7;

        private Rigidbody _rb;
        private Vector2 _moveInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _inputActions = new InputSystem_Actions();

            if (_camera == null)
                _camera = GetComponentInChildren<Camera>();

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();

            // --- A04 EXTRA CHALLENGE: Log "Attack!! (count)" ---
            _inputActions.Player.Attack.performed += ctx => {
                _attackCount++;
                Debug.Log($"Attack!! ({_attackCount})");
            };
        }

        private void OnDisable() => _inputActions.Player.Disable();

        private void Update()
        {
            // 1. Movement Input
            _moveInput = _inputActions.Player.Move.ReadValue<Vector2>();

            // 2. Jump Input
            if (_inputActions.Player.Jump.triggered) TryJump();

            // 3. Look Input (Assuming you added 'Look' as Mouse/Pointer Delta)
            Vector2 lookInput = _inputActions.Player.Look.ReadValue<Vector2>();
            float yaw = lookInput.x * sensitivityX;
            float pitch = -lookInput.y * sensitivityY;

            _pitch = Mathf.Clamp(_pitch + pitch, minPitch, maxPitch);

            transform.Rotate(Vector3.up, yaw, Space.Self);
            if (MainCamera != null)
                MainCamera.transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
        }

        private void FixedUpdate() => UpdateLocomotion();

        private void TryJump() { if (!IsGrounded()) return; _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }
        private void UpdateLocomotion() { Vector3 netAccel = Vector3.zero; netAccel += new Vector3(0f, _gravityAccel, 0f); Vector3 inputWorld = (MainCamera != null) ? MainCamera.transform.TransformDirection(_moveInput.x, 0f, _moveInput.y) : transform.TransformDirection(_moveInput.x, 0f, _moveInput.y); bool hasInput = _moveInput.sqrMagnitude > 1e-6f; if (hasInput) netAccel += ComputeMoveAccel(inputWorld); else netAccel += ComputeBrakeAccel(); _rb.AddForce(netAccel, ForceMode.Acceleration); }
        private Vector3 ComputeMoveAccel(Vector3 worldMoveDir) { Vector3 desiredDir = Horizontal(worldMoveDir).normalized; if (desiredDir.sqrMagnitude < 1e-6f) return Vector3.zero; float accel = acceleration * (IsGrounded() ? 1f : 2f); Vector3 vH = Horizontal(_rb.linearVelocity); float dt = Time.fixedDeltaTime; Vector3 proposed = vH + desiredDir * accel * dt; if (proposed.magnitude > maxMoveSpeed) { float keep = Mathf.Max(vH.magnitude, maxMoveSpeed); proposed = proposed.normalized * keep; } Vector3 requiredHorizAccel = (proposed - vH) / dt; return requiredHorizAccel; }
        private Vector3 ComputeBrakeAccel() { if (!IsGrounded()) return Vector3.zero; Vector3 vH = Horizontal(_rb.linearVelocity); if (vH.sqrMagnitude <= 1e-6f) return Vector3.zero; float dt = Time.fixedDeltaTime; float maxBraking = Mathf.Min(brakingFactor, vH.magnitude / dt); return -maxBraking * vH.normalized; }
        private static Vector3 Horizontal(Vector3 v) => Vector3.ProjectOnPlane(v, Vector3.up);
        private bool IsGrounded() { int ignoreMask = ~(1 << ignoreLayer); int mask = groundMask.value & ignoreMask; return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, mask, QueryTriggerInteraction.Ignore); }
    }
}