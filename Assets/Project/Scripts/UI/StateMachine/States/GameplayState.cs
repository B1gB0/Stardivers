namespace Project.Scripts.UI.StateMachine.States
{
    public class GameplayState : ViewState
    {
        private readonly View.View _uiRootButtons;

        public GameplayState(View.View view, View.View uiRootButtons) : base(view)
        {
            _uiRootButtons = uiRootButtons;
        }

        public override void Enter()
        {
            _uiRootButtons.Activate();
            base.Enter();
        }

        public override void Exit()
        {
            _uiRootButtons.Deactivate();
            base.Exit();
        }
    }
}