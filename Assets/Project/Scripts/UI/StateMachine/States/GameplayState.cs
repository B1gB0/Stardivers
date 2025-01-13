using UnityEngine;

namespace Project.Scripts.UI.StateMachine.States
{
    public class GameplayState : ViewState
    {
        private readonly IView _uiRootButtons;

        public GameplayState(IView view, IView uiRootButtons) : base(view)
        {
            _uiRootButtons = uiRootButtons;
        }

        public override void Enter()
        {
            _uiRootButtons.Show();
            base.Enter();
        }

        public override void Exit()
        {
            _uiRootButtons.Hide();
            base.Exit();
        }
    }
}