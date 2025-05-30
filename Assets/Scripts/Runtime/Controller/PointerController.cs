using UnityEngine;
using UnityEngine.InputSystem;

namespace BcoffeeDev.MelonMerge
{
    public class PointerController : BaseController
    {
        protected override void HandleCreate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                var melon = MelonFactory.Instance.GetMelon();
                melon.transform.position = worldPos;
            }
        }

        protected override void HandleMove()
        {
        }
    }
}