namespace Project.Scripts.UI.StateMachine.States
{
    public class MainMenuState : ViewState
    {
        private readonly View.View _uiRootButtons;
        private readonly View.View _mainMenuView;

        public MainMenuState(View.View view, View.View uiRootButtons) : base(view)
        {
            _uiRootButtons = uiRootButtons;
            _mainMenuView = view;
        }

        public override void Enter()
        {
            _uiRootButtons.Activate();
            _mainMenuView.Show();
        }

        public override void Exit()
        {
            _uiRootButtons.Deactivate();
            _mainMenuView.Hide();
        }
    }
}