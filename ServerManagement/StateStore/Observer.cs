namespace ServerManagement.StateStore
{
    public class Observer
    {
        protected Action? _listners;

        public void AddStateChangeListeners(Action? listners)
        {
            if (listners is not null)
                _listners += listners;
        }

        public void RemoveStateChangeListeners(Action? listners)
        {
            if (listners is not null)
                _listners -= listners;
        }

        public void BroadcastStateChange()
        {
            _listners?.Invoke();
        }
    }
}
