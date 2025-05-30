using System;
using System.Collections.Generic;
using UnityEngine;

namespace BcoffeeDev.MelonMerge
{
    [CreateAssetMenu(menuName = "Other/MelonMerge/Create MelonDataContainer", fileName = "MelonDataContainer", order = 0)]
    public class MelonDataContainer : ScriptableObject
    {
        #region Sub-structures

        [Serializable]
        private class MelonDataPair
        {
            [Range(1, 11)] public int Level;
            public MelonData Data;
        }

        #endregion

        [SerializeField] private List<MelonDataPair> dataPairs = new();

        public bool TryGetData(int level, out MelonData data)
        {
            var pair = dataPairs.Find(x => x.Level == level);
            data = pair?.Data;
            return pair != null;
        }
    }
}