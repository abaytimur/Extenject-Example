using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions.Unity
{
    public class MonoPool
    {
        public int ActiveCount { get; private set; }

        private readonly MonoPoolData _monoPoolData;
        private readonly List<PoolObjData> _myPool = new List<PoolObjData>();

        //TODO: Create for local pos and rot
        public MonoPool(MonoPoolData monoPoolData)
        {
            _monoPoolData = monoPoolData;

            if (_monoPoolData.Prefab.TryGetComponent(out IPoolObj _) == false)
            {
                Debug.LogError("This is not a pool object. Make sure you inherit IPoolObj at prefab main parent");
            }

            for (int i = 0; i < _monoPoolData.InitSize; i++)
            {
                Create();
            }
        }

        public void DeSpawn(IPoolObj poolObj)
        {
            for (int i = 0; i < _myPool.Count; i++)
            {
                PoolObjData thisPoolObjData = _myPool[i];

                if (thisPoolObjData.MyPoolObj == poolObj)
                {
                    thisPoolObjData.BeforeDeSpawn();
                    thisPoolObjData.GameObject.SetActive(false);
                    thisPoolObjData.IsActive = false;
                    _myPool[i] = thisPoolObjData;
                    ActiveCount--;
                    break;
                }
            }
        }

        public void DestroyPool()
        {
            _myPool.DoToAll(po => Object.Destroy(po.GameObject));
            _myPool.Clear();
        }

        public void DeSpawnAfterTween(IPoolObj poolObj)
        {
            for (int i = 0; i < _myPool.Count; i++)
            {
                PoolObjData thisPoolObjData = _myPool[i];

                if (thisPoolObjData.MyPoolObj == poolObj)
                {
                    thisPoolObjData.MyPoolObj.TweenDelayedDeSpawn(delegate
                    {
                        OnOprComplete(thisPoolObjData, i);
                        return true;
                    });

                    _myPool[i] = thisPoolObjData;
                    break;
                }
            }
        }

        public void DeSpawnLastAfterTween()
        {
            for (int i = _myPool.Count - 1; i >=  0; i--)
            {
                PoolObjData thisPoolObjData = _myPool[i];

                if (thisPoolObjData.IsActive)
                {
                    thisPoolObjData.MyPoolObj.TweenDelayedDeSpawn(delegate
                    {
                        OnOprComplete(thisPoolObjData, i);
                        return true;
                    });

                    _myPool[i] = thisPoolObjData;
                    break;
                }
            }
        }

        private void OnOprComplete(PoolObjData thisPoolObjData, int i)
        {
            thisPoolObjData.IsActive = false;
            ActiveCount--;
            thisPoolObjData.BeforeDeSpawn();
            thisPoolObjData.GameObject.SetActive(false);
            _myPool[i] = thisPoolObjData;
        }

        public T Request<T>(Transform parent = null, Vector3 worldPos = default, Quaternion worldRot = default) where T : IPoolObj
        {
            if (parent == null)
            {
                parent = _monoPoolData.ParentToInstUnder;
            }

            if (worldPos == default)
            {
                worldPos = _monoPoolData.DefaultCreateWorldPos;
            }

            if (worldRot == default)
            {
                worldRot = _monoPoolData.DefaultCreateWorldRot;
            }

            PoolObjData foundObjData = new PoolObjData();
            int foundAtIndex = -1;

            for (int i = 0; i < _myPool.Count; i++)
            {
                if (_myPool[i].IsActive == false)
                {
                    foundObjData = _myPool[i];
                    foundAtIndex = i;
                    break;
                }
            }

            if (foundAtIndex >= 0)
            {
                foundObjData.GameObject.SetActive(true);
                foundObjData.IsActive = true;

                _myPool[foundAtIndex] = foundObjData;

                if (parent != null)
                {
                    foundObjData.Transform.SetParent(parent);
                }

                foundObjData.Transform.position = worldPos;

                foundObjData.Transform.rotation = worldRot;

                foundObjData.AfterRespawn();
                ActiveCount++;
                return (T)foundObjData.MyPoolObj;
            }

            foundObjData = Create(parent, worldPos, worldRot);
            foundObjData.GameObject.SetActive(true);
            foundObjData.AfterRespawn();
            PoolObjData createdPoolObjData = _myPool.Last();
            createdPoolObjData.IsActive = true;
            _myPool[_myPool.Count - 1] = createdPoolObjData;

            ActiveCount++;
            return (T)foundObjData.MyPoolObj;
        }

        public void Request(Transform parent = null, Vector3 worldPos = default, Quaternion worldRot = default) => Request<IPoolObj>(parent, worldPos, worldRot);

        private PoolObjData Create(Transform parent = null, Vector3 worldPos = default, Quaternion worldRot = default)
        {
            if (parent == null)
            {
                parent = _monoPoolData.ParentToInstUnder;
            }

            if (worldPos == default)
            {
                worldPos = _monoPoolData.DefaultCreateWorldPos;
            }

            if (worldRot == default)
            {
                worldRot = _monoPoolData.DefaultCreateWorldRot;
            }

            GameObject newObj = Object.Instantiate(_monoPoolData.Prefab, worldPos, worldRot, parent);
            IPoolObj newPoolObj = newObj.GetComponent<IPoolObj>();

            PoolObjData newPoolListObjData = new PoolObjData
            (
                newPoolObj
            );

            _myPool.Add(newPoolListObjData);
            newPoolListObjData.AfterCreate();
            newPoolListObjData.GameObject.SetActive(false);
            newPoolListObjData.Transform.position = worldPos;
            newPoolListObjData.Transform.rotation = worldRot;
            newPoolListObjData.AssignPool(this);
            newPoolListObjData.IsActive = false;

            return newPoolListObjData;
        }
    }

    public readonly struct MonoPoolData
    {
        public readonly GameObject Prefab;
        public readonly int InitSize;
        public readonly Transform ParentToInstUnder;
        public readonly Vector3 DefaultCreateWorldPos;
        public readonly Quaternion DefaultCreateWorldRot;

        public MonoPoolData(GameObject prefab, int initSize, Transform parentToInstUnder = null, Vector3 defaultCreateWorldPos = default, Quaternion defaultCreateWorldRot = default)
        {
            Prefab = prefab;

            if (initSize <= 0)
            {
                initSize = 1;
            }

            InitSize = initSize;
            ParentToInstUnder = parentToInstUnder;
            DefaultCreateWorldPos = defaultCreateWorldPos;
            DefaultCreateWorldRot = defaultCreateWorldRot;
        }
    }

    public struct PoolObjData
    {
        public readonly IPoolObj MyPoolObj;
        public readonly Transform Transform;
        public readonly GameObject GameObject;

        public bool IsActive;

        public PoolObjData(IPoolObj myPoolObj)
        {
            MyPoolObj = myPoolObj;
            IsActive = default;
            Transform = myPoolObj.transform;
            GameObject = myPoolObj.gameObject;
        }

        public void AssignPool(MonoPool myPool)
        {
            MyPoolObj.MyPool = myPool;
        }

        public void AfterCreate() => MyPoolObj.AfterCreate();
        public void BeforeDeSpawn() => MyPoolObj.BeforeDeSpawn();
        public void AfterRespawn() => MyPoolObj.AfterSpawn();
    }

    public interface IPoolObj
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        MonoPool MyPool { get; set; }
        void AfterCreate();
        void BeforeDeSpawn();
        void TweenDelayedDeSpawn(Func<bool> onComplete);
        void AfterSpawn();
    }
}