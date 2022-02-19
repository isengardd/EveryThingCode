using System;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectPoolType
{
    PLAYER = 1,
    ENEMY = 2,
    BULLET = 3,
    BULLET_HIT = 4,
    BULLET_FIRE = 5,
}

public class ObjectPoolManager
{
    public static ObjectPoolManager inst { get; } = new ObjectPoolManager();

    private Dictionary<ObjectPoolType, GameObject> dicFactory;
    private Dictionary<ObjectPoolType, ObjPool<GameObject>> dicObjPool;

    private ObjectPoolManager() {
        dicFactory = new Dictionary<ObjectPoolType, GameObject>();
        dicObjPool = new Dictionary<ObjectPoolType, ObjPool<GameObject>>();

        GameObject playerFactory = new GameObject("PlayerFactory");
        playerFactory.SetActive(false);
        UnityEngine.Object.DontDestroyOnLoad(playerFactory);
        dicFactory[ObjectPoolType.PLAYER] = playerFactory;

        GameObject enemyFactory = new GameObject("EnemyFactory");
        enemyFactory.SetActive(false);
        UnityEngine.Object.DontDestroyOnLoad(enemyFactory);
        dicFactory[ObjectPoolType.ENEMY] = enemyFactory;

        GameObject bulletFactory = new GameObject("BulletFactory");
        bulletFactory.SetActive(false);
        UnityEngine.Object.DontDestroyOnLoad(bulletFactory);
        dicFactory[ObjectPoolType.BULLET] = bulletFactory;
        dicFactory[ObjectPoolType.BULLET_HIT] = bulletFactory;
        dicFactory[ObjectPoolType.BULLET_FIRE] = bulletFactory;
    }

    public void InitPool(ObjectPoolType poolType, int count)
    {
        switch (poolType)
        {
            case ObjectPoolType.PLAYER:
                {
                    GameObject playerFactory = dicFactory[ObjectPoolType.PLAYER];
                    GameObject playerPrefab = ResourcePool.inst.playerPrefab;
                    dicObjPool[ObjectPoolType.PLAYER] = new ObjPool<GameObject>(() => {
                        GameObject playerObj = GameObject.Instantiate(playerPrefab);
                        playerObj.name = string.Format("Player");
                        playerObj.tag = CustomTag.PLAYER;
                        playerObj.transform.parent = playerFactory.transform;
                        return playerObj;
                    }, null, count);
                }
                break;
            case ObjectPoolType.ENEMY:
                {
                    GameObject enemyFactory = dicFactory[ObjectPoolType.ENEMY];
                    GameObject enemyPrefab = ResourcePool.inst.enemyPrefab;
                    dicObjPool[ObjectPoolType.ENEMY] = new ObjPool<GameObject>(() => {
                        GameObject enemyObj = GameObject.Instantiate(enemyPrefab);
                        enemyObj.name = string.Format("Enemy");
                        enemyObj.tag = CustomTag.ENEMY;
                        enemyObj.transform.parent = enemyFactory.transform;
                        return enemyObj;
                    }, null, count);
                }
                break;
            case ObjectPoolType.BULLET:
                {
                    GameObject bulletFactory = dicFactory[ObjectPoolType.BULLET];
                    GameObject bulletPrefab = ResourcePool.inst.bulletPrefab;
                    dicObjPool[ObjectPoolType.BULLET] = new ObjPool<GameObject>(() => {
                        GameObject bulletObj = GameObject.Instantiate(bulletPrefab);
                        bulletObj.name = string.Format("Bullet");
                        bulletObj.tag = CustomTag.BULLET;
                        bulletObj.transform.parent = bulletFactory.transform;
                        return bulletObj;
                    }, null, count);
                }
                break;
            case ObjectPoolType.BULLET_HIT:
                {
                    GameObject bulletHitFactory = dicFactory[ObjectPoolType.BULLET_HIT];
                    GameObject bulletHitPrefab = ResourcePool.inst.bulletHitPrefab;
                    dicObjPool[ObjectPoolType.BULLET_HIT] = new ObjPool<GameObject>(() => {
                        GameObject bulletHitObj = GameObject.Instantiate(bulletHitPrefab);
                        bulletHitObj.name = string.Format("BulletHit");
                        bulletHitObj.transform.parent = bulletHitFactory.transform;
                        return bulletHitObj;
                    }, null, count);
                }
                break;
            case ObjectPoolType.BULLET_FIRE:
                {
                    GameObject bulletFireFactory = dicFactory[ObjectPoolType.BULLET_FIRE];
                    GameObject bulletFirePrefab = ResourcePool.inst.bulletFirePrefab;
                    dicObjPool[ObjectPoolType.BULLET_FIRE] = new ObjPool<GameObject>(() => {
                        GameObject bulletFireObj = GameObject.Instantiate(bulletFirePrefab);
                        bulletFireObj.name = string.Format("BulletFire");
                        bulletFireObj.transform.parent = bulletFireFactory.transform;
                        return bulletFireObj;
                    }, null, count);
                }
                break;
            default:
                Debug.Log(string.Format("unhandle PoolType {0}", poolType));
                break;
        }
    }

    public GameObject GetObjectFromPool(ObjectPoolType poolType)
    {
        GameObject obj = dicObjPool[poolType].Get();
        obj.SetActive(true);
        return obj;
    }

    public List<GameObject> GetObjectsFromPool(ObjectPoolType poolType, int count)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        GameObject obj;
        for (int i= 0; i < count; i++)
        {
            obj = dicObjPool[poolType].Get();
            obj.SetActive(true);
            gameObjects.Add(obj);
        }
        return gameObjects;
    }

    public void PushGameObjectToPool(ObjectPoolType poolType, GameObject gameObj)
    {
        gameObj.SetActive(false);
        gameObj.transform.parent = dicFactory[poolType].transform;
        dicObjPool[poolType].Store(gameObj);
    }

    public void ClearPool()
    {
        foreach (var v in dicObjPool)
        {
            while (v.Value.GetObjCount() > 0)
                UnityEngine.Object.Destroy(v.Value.Get());
        }
    }
}
