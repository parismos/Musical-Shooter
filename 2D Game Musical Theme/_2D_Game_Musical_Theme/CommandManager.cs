using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace _2D_Game_Musical_Theme
{
    //Delegate
    public delegate void GameAction(eButtonState buttonState, Vector2 amount);

    public class CommandManager
    {
        private InputListener m_Input;
        private Dictionary<Keys, GameAction> m_KeyBindings = new Dictionary<Keys, GameAction>();

        public CommandManager()
        {
            //Register events with the input listener
            m_Input = new InputListener();

            m_Input.OnKeyDown += this.OnKeyDown;
            m_Input.OnKeyUp += this.OnKeyUp;
            m_Input.OnKeyPressed += this.OnKeyPressed;



        }

        public void Update()
        {
            //Update by polling input listener, everything else is handled by events
            m_Input.Update();
        }

        public void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];
            if (action != null) action(eButtonState.DOWN, new Vector2(1.0f));   
        }

        public void OnKeyUp(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];
            if (action != null) action(eButtonState.UP, new Vector2(1.0f));

        }

        public void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];
            if (action != null) action(eButtonState.PRESSED, new Vector2(1.0f));
        }

        public void AddKeyboardBinding(Keys key, GameAction action)
        {
            //Add key to InputListener
            m_Input.AddKey(key);

            //Add key to binding to the command map
            m_KeyBindings.Add(key, action);

        }



    }


}
