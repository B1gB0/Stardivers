namespace Project.Scripts.UI.StateMachine.States
{
    public class ExitPanelState : ViewState
    {
        private readonly View.View _view;
        
        public ExitPanelState(View.View view) : base(view)
        {
            _view = view;
        }

        public override void Enter()
        {
            _view.Show();
        }

        public override void Exit()
        {
            _view.Hide();
        }
    }
}