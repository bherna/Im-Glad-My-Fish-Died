using Assests.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Assests.Inputs
{


    public class CustomCursorButtons : PlayerInputDispatcher, NewControls.IUIActions
    {

        private void Start()
        {
            InputSystemManager.Subscribe(this);
        }
        private void OnDestroy()
        {
            InputSystemManager.UnSubscribe(this);
        }

        public void OnCancel(InputAction.CallbackContext context) { }

        public void OnLeftClick(InputAction.CallbackContext context) 
        {

            //is the game currently paused
            if (Controller_EscMenu.instance.paused)
            {
                return;
            }

            //spawn pellet
            //if we can buy food, spawn it
            if (Controller_Food.instance)
            {
                Controller_Food.instance.SpawnFood_Pellet(Vector2.zero, true);/*
                if (Controller_Wallet.instance.IsAffordable(5))
                {
                    SpawnFood_Pellet(CustomVirtualCursor.GetMousePosition_V2(), true);

                    //sub money + visual
                    Controller_Wallet.instance.SubMoney(5);
                    Controller_PopUp.instance.CreateTextPopUp(string.Format("- {0}", 5), CustomVirtualCursor.MousePosition);
                }*/
            }
            
        }

        public void OnMiddleClick(InputAction.CallbackContext context) { }

        public void OnNavigate(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        public void OnRightClick(InputAction.CallbackContext context) { }

        public void OnScrollWheel(InputAction.CallbackContext context) { }

        public void OnSubmit(InputAction.CallbackContext context) { }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }





    }

}