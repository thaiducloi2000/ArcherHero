using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Pool;
using UnityEngine.Windows;

public class TestPlayerController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 10.0f;
    [SerializeField] private Transform TankTurret;
    [SerializeField] private Transform ShootPoint;
    //[SerializeField] private Projectile ProjectilePreference;
    public float SpeedChangeRate = 10.0f;
    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Shoot speed of the Tank in Amount / s, Ex: 1 => 1 Shoot / Second")]
    [Range(0.1f, 5f)]
    public float SpeedShoot;
    public Camera Main => Camera.main;
    public Vector3 Position => transform.position;
    private Ray rayLine;
    private Plane plane;
    private Vector3 position;
    private PlayerInputEvent Event;
    private InputParam MoveInput;

    //private IObjectPool<Projectile> PoolBullet;
    //private Projectile m_projectile;
    private bool m_BlockShoot = false;

    private void Awake()
    {
        //PoolBullet = new ObjectPool<Projectile>(() => CreatePooledItem(ProjectilePreference), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
    }

    private void OnEnable()
    {
        Event = GameplayController.Instance.PlayerInputEvent;
        Setup();
    }

    private void OnDisable()
    {
        RemoveListener();
        Event = null;
    }

    private void RemoveListener()
    {
        Event.RemoveListener<InputParam>((int)PlayerInputID.LocalPlayerMove, OnPlayerMove);
        Event.RemoveListener<InputParam>((int)PlayerInputID.LocalPlayerRotate, OnPlayerRotate);
        Event.RemoveListener<InputParam>((int)PlayerInputID.LocalPlayerShoot, OnPlayerShoot);
    }


    private void Setup()
    {
        MoveInput = new InputParam();
        plane = new Plane(Vector3.up, Position.y);
        m_BlockShoot = false;

        Event.AddListener<InputParam>((int)PlayerInputID.LocalPlayerMove, OnPlayerMove);
        Event.AddListener<InputParam>((int)PlayerInputID.LocalPlayerRotate, OnPlayerRotate);
        Event.AddListener<InputParam>((int)PlayerInputID.LocalPlayerShoot, OnPlayerShoot);
    }

    private void Update()
    {
        Move();
    }
    #region Logic
    private void Move()
    {
        //float targetSpeed = MoveSpeed;
        //// set target speed based on move speed, sprint speed and if sprint is pressed
        //if (Input.Move == Vector2.zero) targetSpeed = 0.0f;
        //Vector3 direction = new Vector3(Input.Move.x, 0f, Input.Move.y);
        //transform.position += direction * targetSpeed * Time.deltaTime;
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (MoveInput.Move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(MoveInput.Move.x, 0.0f, MoveInput.Move.y).magnitude * 5f;

        float speedOffset = 0.1f;
        float inputMagnitude = /*_input.analogMovement ? _input.move.magnitude :*/ 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // normalise input direction
        Vector3 inputDirection = new Vector3(MoveInput.Move.x, 0.0f, MoveInput.Move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (MoveInput.Move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              Main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        Vector3 direction = targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
        // move the player
        transform.position += direction;
    }

    private void RotateShootPoint()
    {
        //if (!transform.CalculateTankRotate(MoveInput.MousePosition, out Quaternion rotation)) return;
        //TankTurret.rotation = Quaternion.Slerp(TankTurret.rotation, rotation, RotationSmoothTime);
    }

    private void Shoot()
    {
        //if (m_BlockShoot) return;
        //m_projectile = PoolBullet.Get();
        //// test Data
        //ProjectileData m_projectileData = new ProjectileData();
        //m_projectileData.Source = this;
        //m_projectileData.Pool = PoolBullet;
        //m_projectileData.StartPosition = ShootPoint.position;
        //m_projectileData.Direction = ShootPoint.forward;
        //m_projectileData.Range = 30f;
        //m_projectileData.FlySpeed = 30f;
        //// end test
        //m_projectile.Setup(m_projectileData);
        ////m_BlockShoot = true;
        //m_projectile.StartFly();
        ////Invoke(nameof(EnableShoot), 1 / SpeedShoot);
    }

    private void EnableShoot()
    {
        m_BlockShoot = false;
    }

    #endregion
    #region Listener
    private void OnPlayerMove(InputParam param)
    {
        MoveInput.Move = param.Move;
    }

    private void OnPlayerRotate(InputParam param)
    {
        MoveInput.MousePosition = param.MousePosition;
        RotateShootPoint();
    }

    private void OnPlayerShoot(InputParam param)
    {
        MoveInput.IsShoot = param.IsShoot;
        if (!MoveInput.IsShoot) return;
        Shoot();
    }
    #endregion

    #region Pool
    //private Projectile CreatePooledItem(Projectile prefabs)
    //{
    //    Projectile projectile = Instantiate(prefabs);
    //    return projectile;
    //}

    //private void OnReturnedToPool(Projectile weapon)
    //{
    //    weapon.gameObject.SetActive(false);
    //}

    //private void OnTakeFromPool(Projectile weapon)
    //{
    //    weapon.gameObject.SetActive(true);
    //}

    //private void OnDestroyPoolObject(Projectile weapon)
    //{
    //    Destroy(weapon.gameObject);
    //}
    #endregion
}
