using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IDestroeable, IDamageable
{
    public event Action OnDie;
    public bool Dirty { get; set; }
    public Joystick _joystick;

    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Bomb _bomb;
    
    private Rigidbody _rb;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _movingDirection; 
    private Vector3 movement;
    public Bomb _previousBomb;
    private BombButton _bombButton;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Subscribe(BombButton _bombButton)
    {
        this._bombButton = _bombButton;
        _bombButton.OnClickButton += SetPlant;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + movement * _moveSpeed * Time.fixedDeltaTime);   
    }

    public void SetPlant()
    {
        if(_previousBomb == null)
                            {
            _previousBomb = Instantiate(_bomb);
            Vector3 createPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            _previousBomb.transform.position = createPos;
            _previousBomb.SetPlant(2f);
        }
    }

    private void Update()
    {
        if(_joystick != null)
        {
            movement.x = _joystick.Horizontal;
            movement.y = _joystick.Vertical;

            if(Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                if (movement.x > 0)
                    _spriteRenderer.sprite = _movingDirection[0];
                else if (movement.x < 0)
                    _spriteRenderer.sprite = _movingDirection[1];
            }

            if(Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
            {
                if (movement.y < 0)
                    _spriteRenderer.sprite = _movingDirection[3];
                else if (movement.y > 0)
                    _spriteRenderer.sprite = _movingDirection[2];
            }
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) && _previousBomb == null)
            {
                _previousBomb = Instantiate(_bomb);
                Vector3 createPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                _previousBomb.transform.position = createPos;
                _previousBomb.SetPlant(2f);
            }

            if (movement.x > 0)
                _spriteRenderer.sprite = _movingDirection[0];
            else if (movement.x < 0)
                _spriteRenderer.sprite = _movingDirection[1];
            else if (movement.y < 0)
                _spriteRenderer.sprite = _movingDirection[3];
            else if (movement.y > 0)
                _spriteRenderer.sprite = _movingDirection[2];
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            OnDie?.Invoke();
        }
    }

    public void Resycle()
    {
        _joystick = null;
        _bombButton.OnClickButton -= SetPlant;
        Destroy(gameObject);
        Destroy(this);
    }

    public void MakeYouDirtyOrCleanUP()
    {
        //не знал, что делать со свиньей, перезапустить лвл? спрайтов ее в грязи нету
    }
}
