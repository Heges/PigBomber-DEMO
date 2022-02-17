using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameContent : MonoBehaviour, IDestroeable, IDamageable
{
    public float _moveSpeed;
    public event Action OnDie;

    public LayerMask enemyLayer;
    public Pathfinding _pathfinder;

    private IEnumerator _movingCoroutine;
    private IEnumerator _scavengeCoroutine;
    private List<Node> path;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite[] _movingDirection; 
    [SerializeField] private Sprite[] _movingDirectionAngry; 
    [SerializeField] private Sprite[] _dirtIdle;
    private Sprite[] _currentMoveList;

    [SerializeField] FaceDirection _faceDirection;
    [SerializeField] float _radiusDetection;

    public bool Dirty
    {
        get;
        set;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _faceDirection.lookDirection = Vector3.right;
        _currentMoveList = _movingDirection;
    }

    private void Start()
    {
        if (_scavengeCoroutine == null)
        {
            _scavengeCoroutine = ScavengeMode();
            StartCoroutine(_scavengeCoroutine);
        }
    }


    private void Update()
    {
        if (!Dirty)
        {
            SimpleDecision();
        }
    }

    [SerializeField] private float timerFounder;
    public void SimpleDecision()
    {
        Collider[] cooliders =  Physics.OverlapSphere(transform.position, _radiusDetection, enemyLayer);
        if (cooliders.Length > 0)
        {
            if (Vector3.Dot(_faceDirection.lookDirection, (cooliders[0].transform.position - transform.position)) > 0.8f)
            {
                Debug.DrawLine(transform.position, cooliders[0].transform.position);

                _currentMoveList = _movingDirectionAngry;
                if (_movingCoroutine == null)
                {
                    _pathfinder.FindPath(transform.position, cooliders[0].transform.position, MoveExecute);
                }
                else if(timerFounder > 0.5f)
                {
                    timerFounder = 0;
                    StopCoroutine(_movingCoroutine);
                    _movingCoroutine = null;
                    _pathfinder.FindPath(transform.position, cooliders[0].transform.position, MoveExecute);
                }
                else
                {
                    timerFounder += Time.deltaTime;
                }
            }
            else
            {
                _currentMoveList = _movingDirection;
            }
        }
        else
        {
            if(_scavengeCoroutine == null)
            {
                _scavengeCoroutine = ScavengeMode();
                StartCoroutine(_scavengeCoroutine);
            }
        }
    }

    public void MoveExecute(List<Node> _path)
    {
        if (_movingCoroutine == null)
        {
            path = _path;
            _movingCoroutine = Moving();
            StartCoroutine(_movingCoroutine);
        }
    }

    [SerializeField] float pauseForScavenge = 5f;
    private IEnumerator ScavengeMode()
    {
        if(_movingCoroutine == null)
        {
            _currentMoveList = _movingDirection;
            Vector3 randomPosition = _pathfinder.GetRandomPosition();
            _pathfinder.FindPath(transform.position, randomPosition, MoveExecute);
        }
        yield return new WaitForSeconds(pauseForScavenge);
        _scavengeCoroutine = null;
    }

    private IEnumerator Moving()
    {
        int index = path.Count - 1;
        var currentNode = path[index];
        var endNode = path[0];
        
        while (true)
        {
            Vector3 newPos = new Vector3(currentNode.position.x, currentNode.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * _moveSpeed);

            CheckRotation(newPos, _currentMoveList);
            if (transform.position.x == newPos.x && transform.position.y == newPos.y) 
            {
                index--;
                if (index <= 0)
                {
                    break;
                }
                    
                currentNode = path[index];
            }

            if(transform.position.x == endNode.position.x && transform.position.y == endNode.position.y)
            {
                break;
            }
            yield return null;
        }
        _movingCoroutine = null;
        _currentMoveList = _movingDirection;
    }

    private void StopMoving()
    {
        Vector3 velocity = _faceDirection.lookDirection;

        CheckRotation(velocity, _dirtIdle);

        if (_scavengeCoroutine != null)
        {
            StopCoroutine(_scavengeCoroutine);
            _scavengeCoroutine = null;

        }
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
        
    }

    private void CheckRotation(Vector3 movement, Sprite[] _array)
    {
        Vector3 direction = movement - transform.position;

        if (direction.x > 0)
        {
            _spriteRenderer.sprite = _array[0];
            _faceDirection.lookDirection = Vector3.right;
        }
        else if (direction.x < 0)
        {
            _spriteRenderer.sprite = _array[1];
            _faceDirection.lookDirection = Vector3.left;
        }
        else if (direction.y < 0)
        {
            _spriteRenderer.sprite = _array[3];
            _faceDirection.lookDirection = Vector3.down;
        }
        else if (direction.y > 0)
        {
            _spriteRenderer.sprite = _array[2];
            _faceDirection.lookDirection = Vector3.up;
        }
    }

    public void Resycle()
    {
        Destroy(gameObject);
        Destroy(this);
    }

    public void MakeYouDirtyOrCleanUP()
    {
        if (!Dirty)
        {
            Dirty = true;
            StartCoroutine(CleanSelf(2f));
        }
    }

    private IEnumerator CleanSelf(float _timeToClean)
    {
        StopMoving();
        yield return new WaitForSeconds(_timeToClean);
        Dirty = false;
    }
}
