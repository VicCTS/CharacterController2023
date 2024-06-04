using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricController : MonoBehaviour
{
    private CharacterController _controller;
    private float _horizontal;
    private float _vertical;

    //variables para velocidad, salto y gravedad
    [SerializeField] private float _playerSpeed = 5;
    [SerializeField] private float _jumpHeight = 1;
    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    //variables para rotacion
    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;

    //varibles para sensor
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;


    public Camera camaraPrincipal;
    public Transform objetoRaycast;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        Movement();
        Jump();

        if(Input.GetButtonDown("Submit"))
        {
            CameraRay();
        }
        
    }

    void CameraRay()
    {
        Ray ray = camaraPrincipal.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.transform.gameObject.tag == "Objeto 1")
            {
                Debug.Log("dewSGRDF");
            }
            else if(hit.transform.gameObject.tag == "Objeto 2")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetButtonDown("Submit"))
        {
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point);

                /*Vector3 directionRaycast = hit.point - transform.position;
                directionRaycast.y = 0;
                transform.forward = directionRaycast;*/

                if(hit.transform.gameObject.layer == 7)
                {
                    Debug.Log("szssdfsdf");

                    Box box = hit.transform.gameObject.GetComponent<Box>();

                    if(box != null)
                    {
                        box.TakeDamage(2);
                    }
                }
            }
        }
        

        if(direction != Vector3.zero)
        {
            /*float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);*/

            _controller.Move(direction.normalized * _playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {

        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = 0;
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        
        _controller.Move(_playerGravity * Time.deltaTime);
    }
}
