using System;
using UnityEngine;

namespace BcoffeeDev.MelonMerge
{
    [Serializable]
    public class MelonData
    {
        public float Size;
        public float GravityScale;
        public float Mass;
        public Sprite Skin;
        public PhysicsMaterial2D PhysicsMaterial;
    }
}