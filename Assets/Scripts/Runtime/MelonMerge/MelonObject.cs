using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BcoffeeDev.MelonMerge
{
    public class MelonObject : MonoBehaviour
    {
        public int Level { get; private set; }
        public long Id { get; private set; }
        
        private static long NextId = 0;
        private static event Action<int> OnMelonLevelChanged;

        private const int MIN_LEVEL = 1;
        private const int MAX_LEVEL = 10;
        private const int MIN_RANDOM_LEVEL = 1;
        private const int MAX_RANDOM_LEVEL = 5;
        
        #region Unity methods

        private void Start()
        {
            Level = Random.Range(0, 2);
            Init();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out MelonObject melonObj))
                return;

            if (Level != melonObj.Level)
                return; // not the same level obj
            
            // todo merge to bigger one
            var destroyTarget = Id > melonObj.Id ? melonObj : this;
            var aliveTarget = Id > melonObj.Id ? this : melonObj;
            
            var newX = (aliveTarget.transform.position.x + destroyTarget.transform.position.x) / 2;
            var newY = (aliveTarget.transform.position.y + destroyTarget.transform.position.y) / 2;
            aliveTarget.transform.position = new Vector3(newX, newY, aliveTarget.transform.position.z);
            
            if (aliveTarget.TryGetComponent(out Rigidbody2D rigidBody))
                rigidBody.linearVelocity = Vector2.zero;
            
            aliveTarget.LevelUp();
            Destroy(destroyTarget.gameObject);
        }

        #endregion

        public void Init()
        {
            Id = NextId;
            NextId++;
            Level = Random.Range(MIN_RANDOM_LEVEL, MAX_RANDOM_LEVEL);
        }

        public void LevelUp()
        {
            Level = Mathf.Clamp(Level + 1, MIN_LEVEL, MAX_LEVEL);
            OnMelonLevelChanged?.Invoke(Level);
        }
    }
}