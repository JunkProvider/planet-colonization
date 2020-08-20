namespace SpaceLogistic.WpfView.ViewModel
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using SpaceLogistic.WpfView.Annotations;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            this.SetPropertyAffecting(ref field, value, propertyName);
        }

        [NotifyPropertyChangedInvocator]
        protected void SetPropertyAffecting<T>(ref T field, T value, params string[] propertyNames)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;

            foreach (var propertyName in propertyNames)
            {
                this.OnPropertyChanged(propertyName);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}