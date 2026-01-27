namespace Project.Scripts.UI.StateMachine.States
{
    public class ChoosingOperationPanelState : ViewState
    {
        private readonly View.View _view;
        
        public ChoosingOperationPanelState(View.View view) : base(view)
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