using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isPlay = false;
    public Player _player;
    public GameObject _enemy;
    public Player _player1;
    public SessionData session;

    private List<LevelData> levels;
    private void Start()
    { 
        InitLevel();
        UpdateLevel(PlayerPrefs.GetInt(session.nameSession,0));
        EventDispatcher.Instance.RegisterListener(EventID.UpdateLevel,param => UpdateLevel(PlayerPrefs.GetInt(session.nameSession,0)));
    }

    private void InitLevel()
    {
        levels = session.levelDatas;
    }
    public void UpdateLevel(int id)
    {
        if (id < session.levelDatas.Count)
        {
            UIGame.Instance.TxtNotiLevel("Level "+(id+1));
            DesspawnAllEnemy();
            if (_player1 != null)
            {
                PoolingManager.Despawn(_player1.gameObject);
            }
            GridManager.Instance.InitializeGrid(levels[id]);
            EventDispatcher.Instance.PostEvent(EventID.OnUpdateCenterGrid);
            _player1 = PoolingManager.Spawn(_player,GridManager.Instance.levelData.startPoint, quaternion.identity);
            _player1.InitPlayer();

            foreach (var posenm in GridManager.Instance.levelData.posEnemy )
            {
                PoolingManager.Spawn(_enemy, posenm, quaternion.identity);
            }
            EventDispatcher.Instance.RegisterListener(EventID.OnUpdatePath,param => _player1.UpdatePath());
            EventDispatcher.Instance.RegisterListener(EventID.OnPlayerMove,param => _player1.isMove = true);   
        }
        else
        {
            UIGame.Instance.txtLevel.color = Color.green;
            UIGame.Instance.TxtNotiLevel("Bạn đã Thắng");
        }
    }

    void DesspawnAllEnemy()
    {
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enm in allEnemy)
        {
            Destroy(enm);
        }
    }
    
}
