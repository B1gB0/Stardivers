using UnityEngine;

namespace Project.Scripts.UI.StateMachine.States
{
    public abstract class ViewState : IUIState
    {
        private readonly IView _view;

        protected ViewState(IView view)
        {
            _view = view;
        }

        public virtual void Enter()
        {
            _view.Show();
        }

        public virtual void Exit()
        {
            _view.Hide();
        }
    }
}