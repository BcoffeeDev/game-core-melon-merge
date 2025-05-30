using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace BcoffeeDev.MelonMerge
{
    public class MelonFactory : MonoBehaviour
    {
        [SerializeField] private MelonDataContainer dataContainer;
        [SerializeField] private MelonObject melonPrefab;
        
        private IObjectPool<MelonObject> _pool;
        
        public static MelonFactory Instance;
        
        public MelonDataContainer DataContainer => dataContainer;

        private readonly HashSet<(MelonObject, MelonObject)> _pendingMerges = new();

        #region Unity methods

        private void Awake()
        {
            Instance = this;
            _pool = new ObjectPool<MelonObject>(CreatePoolMelon, GetPoolMelon, ReleasePoolMelon, DestroyPoolMelon);
        }

        private void LateUpdate()
        {
            foreach (var (a, b) in _pendingMerges)
            {
                if (!IsMergeable(a, b))
                    continue;

                var release = a.Id > b.Id ? a : b;
                var keep = a.Id > b.Id ? b : a;

                var newX = (a.transform.position.x + b.transform.position.x) / 2;
                var newY = (a.transform.position.y + b.transform.position.y) / 2;
                keep.transform.position = new Vector3(newX, newY, keep.transform.position.z);

                if (keep.TryGetComponent(out Rigidbody2D rb))
                    rb.linearVelocity = Vector2.zero;

                keep.LevelUp();
                ReleaseMelon(release);
            }

            _pendingMerges.Clear();
        }

        #endregion

        #region Pooling

        private MelonObject CreatePoolMelon()
        {
            var obj = Instantiate(melonPrefab, transform);
            return obj;
        }

        private void GetPoolMelon(MelonObject obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void ReleasePoolMelon(MelonObject obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void DestroyPoolMelon(MelonObject obj)
        {
            Destroy(obj.gameObject);
        }

        #endregion

        public MelonObject GetMelon()
        {
            var melon = _pool.Get();
            melon.Init();
            return melon;
        }

        public void ReleaseMelon(MelonObject obj)
        {
            obj.Deactivate();
            _pool.Release(obj);
        }

        public void RegisterMergeRequest(MelonObject a, MelonObject b)
        {
            if (!a.Activated || !b.Activated) return;
            if (a.Level != b.Level || a.Level == MelonObject.MAX_LEVEL) return;

            var pair = a.Id > b.Id ? (b, a) : (a, b);
            _pendingMerges.Add(pair);
        }

        private bool IsMergeable(MelonObject a, MelonObject b)
        {
            if (!a || !b) return false;
            if (!a.Activated || !b.Activated) return false;
            if (a.Level != b.Level || a.Level == MelonObject.MAX_LEVEL) return false;
            return true;
        }
    }
}