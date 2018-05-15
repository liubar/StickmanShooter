using UnityEngine;

namespace App
{
    public class MoveController : MonoBehaviour, IMoveController
    {
        [SerializeField]
        TouchButton leftButton;
        [SerializeField]
        TouchButton rightButton;
        [SerializeField]
        string axisName = "HorizontalP1";

        public float HorizontalInput
        {
            get
            {
                if (leftButton.CurrentState == ButtonState.Held || leftButton.CurrentState == ButtonState.PressedDown)
                {
                    return -1;
                }
                else if (rightButton.CurrentState == ButtonState.Held || rightButton.CurrentState == ButtonState.PressedDown)
                {
                    return 1;
                }

                return Input.GetAxis(axisName);
            }
        }
    }
}
