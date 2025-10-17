using Project.Scripts.UI.View;

namespace Project.Scripts.UI.StateMachine.States
{
    public abstract class ViewState : UIState
    {
        private readonly IView _view;

        protected ViewState(IView view)
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