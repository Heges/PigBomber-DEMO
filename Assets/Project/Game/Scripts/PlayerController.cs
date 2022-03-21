using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : WorldObject, IDestroyable
{
    public event Action OnDie;
    public Joystick joystick;

    [SerializeField] private float moveSpeed;
    [SerializeField] private EventVoidAction OnPlantAction;
    [SerializeField] private Bomb bomb;
    [SerializeField] private Sprite[] _movingDirection;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 movement;
    private Bomb previousBomb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        OnPlantAction.OnAction += SetPlant;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);   
    }

    public void SetPlant()
    {
        if(previousBomb == null)
        {
            previousBomb = Instantiate(bomb);
            Vector3 createPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            previousBomb.transform.position = createPos;
            previousBomb.SetPlant(2f);
        }
    }

    private void Update()
    {
        if(joystick != null)
        {
            movement.x = joystick.Horizontal;
            movement.y = joystick.Vertical;

            if(Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                if (movement.x > 0)
                    spriteRenderer.sprite = _movingDirection[0];
                else if (movement.x < 0)
                    spriteRenderer.sprite = _movingDirection[1];
            }

            if(Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
            {
                if (movement.y < 0)
                    spriteRenderer.sprite = _movingDirection[3];
                else if (movement.y > 0)
                    spriteRenderer.sprite = _movingDirection[2];
            }
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) && previousBomb == null)
            {
                SetPlant();
            }

            if (movement.x > 0)
                spriteRenderer.sprite = _movingDirection[0];
            else if (movement.x < 0)
                spriteRenderer.sprite = _movingDirection[1];
            else if (movement.y < 0)
                spriteRenderer.sprite = _movingDirection[3];
            else if (movement.y > 0)
                spriteRenderer.sprite = _movingDirection[2];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (previousBomb)
                previousBomb.MustDestroy();
            GameOver();
        }
    }

    private void GameOver()
    {
        OnPlantAction.OnAction -= SetPlant;
        OnDie?.Invoke();
        OnDie = null;
        joystick = null;
    }

    public void MustDestroy()
    {
        Reclaim();
    }
}
