using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile currentTile;
    public List<Tile> listExitTile;
    public List<Tile> path = new List<Tile>();
    public int indexTarget;
    public bool isMove = false;
    private Animator animator;

    public void InitPlayer()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTile = GridManager.Instance.grid[(int) GridManager.Instance.levelData.startPoint.x,(int) GridManager.Instance.levelData.startPoint.y];
        listExitTile.Add(GridManager.Instance.grid[(int) GridManager.Instance.levelData.endPoint.x,(int) GridManager.Instance.levelData.endPoint.y]);
        path = FindingPath.FindPath(currentTile, listExitTile);
        transform.parent = currentTile.transform;
    }

    private void Update()
    {
        if (isMove)
        {
            if (path != null)
            {
                Move();   
            }

            else
            {
                if (GameManager.Instance.isPlay)
                {
                    EventDispatcher.Instance.PostEvent(EventID.OnNotification);   
                }
                isMove = false;
            }
        }
    }

    public void UpdatePath()
    {
        indexTarget = 0;
        path = FindingPath.FindPath(currentTile, listExitTile);
    }



    private void Move()
    {
        if (indexTarget < path.Count)
        {
            Tile targetTile = path[indexTarget];
            if (targetTile != null)
            {
                Vector3 direction = (targetTile.transform.position - transform.position).normalized;
                Vector3 movement = direction * (2f * Time.deltaTime);
                
                animator.SetFloat("MoveX", direction.x);
                animator.SetFloat("MoveY", direction.y);

                transform.position += movement;

                if (Vector2.Distance(transform.position, targetTile.transform.position) < 0.03f)
                {
                    indexTarget++;
                    if (indexTarget >= path.Count)
                    {
                        PlayerPrefs.SetInt(GameManager.Instance.session.nameSession,PlayerPrefs.GetInt(GameManager.Instance.session.nameSession,0)+1);
                        PlayerPrefs.Save();
                        EventDispatcher.Instance.PostEvent(EventID.UpdateLevel,PlayerPrefs.GetInt(GameManager.Instance.session.nameSession,0));
                        path = null;
                        indexTarget = 0;
                        isMove = false;
                    }
                }
            }
        }
        else
        {
            path = null;
            indexTarget = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentTile = GridManager.Instance.grid[(int) GridManager.Instance.levelData.startPoint.x,(int) GridManager.Instance.levelData.startPoint.y];
            transform.position = currentTile.transform.position;
            isMove = false;
            EventDispatcher.Instance.PostEvent(EventID.OnColliderWithEnemy);
        }
    }
}