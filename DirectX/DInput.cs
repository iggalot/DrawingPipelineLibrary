using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace DrawingPipelineLibrary.DirectX
{
    public class DInput                 // 31 lines
    {
        // Variables.
        private Dictionary<Keys, bool> InputKeys = new Dictionary<Keys, bool>();

        // Methods.
        internal void Initialize()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                InputKeys[(Keys)key] = false;
        }
        internal bool IsKeyDown(Keys key)
        {
            return InputKeys[key];
        }
        internal void KeyDown(Keys key)
        {
            InputKeys[key] = true;
        }
        internal void KeyUp(Keys key)
        {
            InputKeys[key] = false;
        }

        /// <summary>
        /// Converts from a WindowsForms key argument to an Windows.Input keys argument
        /// </summary>
        /// <param name="key">Windows Forms / WPF key stroke that was received</param>
        /// <param name="v">boolean for the toggle</param>
        public void SetKeyState(Key key, bool v)
        {
            if (v == true)
                KeyDown((Keys)KeyInterop.VirtualKeyFromKey(key));
            else
                KeyUp((Keys)KeyInterop.VirtualKeyFromKey(key));
        }
    }
}
