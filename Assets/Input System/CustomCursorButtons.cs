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
    
        }

        public void OnMiddleClick(InputAction.CallbackContext context) { }

        public void OnNavigate(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        public void OnRightClick(InputAction.CallbackContext context) 
        {
            
           
        
        }

        public void OnScrollWheel(InputAction.CallbackContext context) { }

        public void OnSubmit(InputAction.CallbackContext context) { }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }

}