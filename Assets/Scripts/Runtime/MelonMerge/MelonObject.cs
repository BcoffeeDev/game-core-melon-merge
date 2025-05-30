using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BcoffeeDev.MelonMerge
{
    public class MelonObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public bool Activated { get; private set; }
        public int Level { get; private set; }
        public long Id { get; private set; }
        
        private static long NextId = 0;
        private static event Action<int> OnMelonLevelChanged;

        public const int MIN_LEVEL = 1;
        public const int MAX_LEVEL = 11;
        public const int MIN_RANDOM_LEVEL = 1;
        public const int MAX_RANDOM_LEVEL = 5;
        
        #region Unity methods

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out MelonObject otherMelon))
            {
                MelonFactory.Instance.RegisterMergeRequest(this, otherMelon);
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out MelonObject otherMelon))
            {
                MelonFactory.Instance.RegisterMergeRequest(this, otherMelon);
            }
        }

        #endregion

        public void Init()
        {
            Id = NextId;
            NextId++;
            Level = Random.Range(MIN_RANDOM_LEVEL, MAX_RANDOM_LEVEL);
            Setup();
            Activate();
        }

        public void Activate()
        {
            Activated = true;
        }

        public void Deactivate()
        {
            Activated = false;
        }

        public void LevelUp()
        {
            var previousLevel = Level;
            Level = Mathf.Clamp(Level + 1, MIN_LEVEL, MAX_LEVEL + 1);
            if (Level != previousLevel)
                Setup();
            OnMelonLevelChanged?.Invoke(Level);
        }

        private void Setup()
        {
            if (!MelonFactory.Instance.DataContainer.TryGetData(Level, out var data))
                return;
            body.gravityScale = data.GravityScale;
            body.mass = data.Mass;
            body.sharedMaterial = data.PhysicsMaterial;
            spriteRenderer.sprite = data.Skin;
            transform.localScale = Vector3.one * data.Size;
        }
    }
}