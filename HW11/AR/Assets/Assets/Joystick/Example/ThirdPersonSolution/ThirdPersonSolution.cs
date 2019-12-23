// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace zFrame.Example
{
    using UnityEngine;
    using zFrame.UI;
    public class ThirdPersonSolution : MonoBehaviour
    {
        [SerializeField] Joystick joystick;
        public float speed = 3;
        private Animation _animation;
        CharacterController controller;
        void Start()
        {
            controller = GetComponent<CharacterController>();
            _animation = GetComponent<Animation>();
            joystick.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    _animation.PlayQueued("sj001_run", QueueMode.CompleteOthers, PlayMode.StopSameLayer);
                    Vector3 direction = new Vector3(v.x, v.y, 0);
                    controller.Move(direction * speed * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(new Vector3(v.x, 0, v.y));
                }
            });
        }
    }
}
