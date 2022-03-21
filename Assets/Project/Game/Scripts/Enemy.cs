using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : WorldObject
{
    public float moveSpeed;

    public LayerMask enemyLayer;

    [SerializeField] private Sprite[] movingDirection; 
    [SerializeField] private Sprite[] movingDirectionAngry; 
    [SerializeField] private Sprite[] dirtIdle;
    [SerializeField] private FaceDirection faceDirection;
    [SerializeField] private float radiusDetection;

    private IEnumerator movingCoroutine;
    private IEnumerator scavengeCoroutine;
    private List<Node> path;

    private SpriteRenderer spriteRenderer;

    private Sprite[] currentMoveList;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        faceDirection.lookDirection = Vector3.right;
        currentMoveList = movingDirection;
    }

    private void Start()
    {
        if (scavengeCoroutine == null)
        {
            scavengeCoroutine = ScavengeMode();
            StartCoroutine(scavengeCoroutine);
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
        Collider[] cooliders =  Physics.OverlapSphere(transform.position, radiusDetection, enemyLayer);
        if (cooliders.Length > 0)
        {
            if (Vector3.Dot(faceDirection.lookDirection, (cooliders[0].transform.position - transform.position)) > 0.8f)
            {
                Debug.DrawLine(transform.position, cooliders[0].transform.position);

                currentMoveList = movingDirectionAngry;
                if (movingCoroutine == null)
                {
                    Pathfinding.instance.FindPath(transform.position, cooliders[0].transform.position, MoveExecute);
                }
                else if(timerFounder > 0.5f)
                {
                    timerFounder = 0;
                    StopCoroutine(movingCoroutine);
                    movingCoroutine = null;
                    Pathfinding.instance.FindPath(transform.position, cooliders[0].transform.position, MoveExecute);
                }
                else
                {
                    timerFounder += Time.deltaTime;
                }
            }
            else
            {
                currentMoveList = movingDirection;
            }
        }
        else
        {
            if(scavengeCoroutine == null)
            {
                scavengeCoroutine = ScavengeMode();
                StartCoroutine(scavengeCoroutine);
            }
        }
    }

    public void MoveExecute(List<Node> _path)
    {
        if (movingCoroutine == null)
        {
            path = _path;
            movingCoroutine = Moving();
            StartCoroutine(movingCoroutine);
        }
    }

    [SerializeField] float pauseForScavenge = 5f;
    private IEnumerator ScavengeMode()
    {
        if(movingCoroutine == null)
        {
            currentMoveList = movingDirection;
            Vector3 randomPosition = Pathfinding.instance.GetRandomPosition();
            Pathfinding.instance.FindPath(transform.position, randomPosition, MoveExecute);
        }
        yield return new WaitForSeconds(pauseForScavenge);
        scavengeCoroutine = null;
    }

    private IEnumerator Moving()
    {
        int index = path.Count - 1;
        var currentNode = path[index];
        var endNode = path[0];
        
        while (true)
        {
            Vector3 newPos = new Vector3(currentNode.position.x, currentNode.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * moveSpeed);

            CheckRotation(newPos, currentMoveList);
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
        movingCoroutine = null;
        currentMoveList = movingDirection;
    }

    private void StopMoving()
    {
        Vector3 velocity = faceDirection.lookDirection;

        CheckRotation(velocity, dirtIdle);

        if (scavengeCoroutine != null)
        {
            StopCoroutine(scavengeCoroutine);
            scavengeCoroutine = null;

        }
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
            movingCoroutine = null;
        }
        
    }

    private void CheckRotation(Vector3 movement, Sprite[] _array)
    {
        Vector3 direction = movement - transform.position;

        if (direction.x > 0)
        {
            spriteRenderer.sprite = _array[0];
            faceDirection.lookDirection = Vector3.right;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.sprite = _array[1];
            faceDirection.lookDirection = Vector3.left;
        }
        else if (direction.y < 0)
        {
            spriteRenderer.sprite = _array[3];
            faceDirection.lookDirection = Vector3.down;
        }
        else if (direction.y > 0)
        {
            spriteRenderer.sprite = _array[2];
            faceDirection.lookDirection = Vector3.up;
        }
    }

    public override void MakeYouDirtyOrCleanUP()
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
