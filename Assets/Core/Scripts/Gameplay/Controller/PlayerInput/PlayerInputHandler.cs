using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public struct InputParam : IBusdata<InputParam>
{
    public Vector2 Move;
    public Vector2 MousePosition;
    public bool IsShoot;
}

public class PlayerInputHandler : MonoBehaviour
{
    [field: SerializeField] public Vector2 Move { get; private set; }
    [field: SerializeField] public Vector2 MousePosition { get; private set; }

    public Action FirstSkillClickCallback;
    public Action SecondSkillClickCallback;

    private PlayerInputEvent InputEvent;
    private InputParam inputParam;
    private bool Shoot;

    private void OnEnable()
    {
        InputEvent = GameplayController.Instance.PlayerInputEvent;
        inputParam = new InputParam();
    }

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnRotate(InputValue value)
    {
        RotateInput(value.Get<Vector2>());
    }
    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }

    public void OnFirstSkill(InputValue value)
    {
        if (!value.isPressed) return;
        FirstSkillClickCallback?.Invoke();
    }

    public void OnSecondSkill(InputValue value)
    {
        if (!value.isPressed) return;
        SecondSkillClickCallback?.Invoke();
    }

    private void MoveInput(Vector2 newMoveDirection)
    {
        Move = newMoveDirection;

        inputParam.Move = Move;
        InputEvent.PostEvent((int)PlayerInputID.LocalPlayerMove, inputParam);
    }

    private void RotateInput(Vector2 newMousePosition)
    {
        MousePosition = newMousePosition;

        inputParam.MousePosition = MousePosition;
        InputEvent.PostEvent((int)PlayerInputID.LocalPlayerRotate, inputParam);
    }

    private void ShootInput(bool isShoot)
    {
        Shoot = isShoot;
        inputParam.IsShoot = Shoot;

        InputEvent.PostEvent((int)PlayerInputID.LocalPlayerShoot, inputParam);
    }
}
