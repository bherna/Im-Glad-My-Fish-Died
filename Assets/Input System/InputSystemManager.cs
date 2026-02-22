using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemManager : MonoBehaviour, NewControls.IButtonsActions, NewControls.IUtilsActions, NewControls.IUIActions
{

    private NewControls inputActions;




    //single ton this class
    public static InputSystemManager instance { get; private set; }

    private void Awake()
    {

        //delete duplicate of this instance
        if (instance == null)
        {

            instance = this;
            inputActions = new NewControls();
            inputActions.Enable();
            inputActions.Buttons.SetCallbacks(this);
            inputActions.Utils.SetCallbacks(this);
            inputActions.UI.SetCallbacks(this);

        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }




    public void OnLeftClick(InputAction.CallbackContext context)
    {

        Debug.LogWarning("OnLeftclick in Manager");
        //proxy to make sure that actions are called at the right times
        //filter events by event type
        if (context.phase == InputActionPhase.Performed)
        {
            foreach (var action in ui_Actions)
            {
                action.OnLeftClick(context);
            }
        }


    }



    public void OnExit(InputAction.CallbackContext context)
    {

        Debug.LogWarning("OnExit in Manager");
        //proxy to make sure that actions are called at the right times
        //filter events by event type
        if (context.phase == InputActionPhase.Performed)
        {
            foreach (var action in buttons_Actions)
            {
                action.OnExit(context);
            }
        }


    }


    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Debug.LogWarning("OnCursorPosition  has been changed");
        //proxy to make sure that actions are called at the right times
        foreach (var action in utils_Actions)
        {
            action.OnCursorPosition(context);
        }

    }

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        Debug.LogWarning("OnCursorMove  has been changed");
        //proxy to make sure that actions are called at the right times
        foreach (var action in utils_Actions)
        {
            action.OnCursorMove(context);
        }
    }






    private static List<NewControls.IButtonsActions> buttons_Actions = new List<NewControls.IButtonsActions>();
    private static List<NewControls.IUtilsActions> utils_Actions = new List<NewControls.IUtilsActions>();
    private static List<NewControls.IUIActions> ui_Actions = new List<NewControls.IUIActions>();


    public static void Subscribe(NewControls.IButtonsActions newAction)
    {
        buttons_Actions.Add(newAction);
    }
    public static void Subscribe(NewControls.IUtilsActions newAction)
    {
        utils_Actions.Add(newAction);
    }
    public static void Subscribe(NewControls.IUIActions newAction)
    {
        ui_Actions.Add(newAction);
    }


    public static void UnSubscribe(NewControls.IButtonsActions newAction)
    {
        buttons_Actions.Remove(newAction);
    }
    public static void UnSubscribe(NewControls.IUtilsActions newAction)
    {
        utils_Actions.Remove(newAction);
    }
    public static void UnSubscribe(NewControls.IUIActions newAction)
    {
        ui_Actions.Remove(newAction);
    }










    public void OnNavigate(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnSubmit(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnCancel(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnPoint(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnScrollWheel(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnMiddleClick(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnRightClick(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { Debug.Log("Not Implemented"); }
}
