// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace zFrame.Example
{
    using UnityEngine;
    using UnityEngine.UI;
    using zFrame.UI;
    public class ConfigurationDemo : MonoBehaviour
    {
        public Joystick joystick;
        Text text;
        public Text text2;
        void Start()
        {
            joystick.DynamicJoystick = false;
        }
        private void Update()
        {
        }
    }
}
