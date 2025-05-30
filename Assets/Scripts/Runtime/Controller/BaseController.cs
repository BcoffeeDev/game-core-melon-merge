using UnityEngine;

namespace BcoffeeDev.MelonMerge
{
    public abstract class BaseController : MonoBehaviour
    {
        #region Unity methods

        private void Update()
        {
            HandleCreate();
            HandleMove();
        }

        #endregion

        protected abstract void HandleCreate();
        protected abstract void HandleMove();
    }
}
