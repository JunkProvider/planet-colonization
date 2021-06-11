namespace SpaceLogistic.WpfView.View
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using SpaceLogistic.WpfView.Properties;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            return this.SetPropertyAffecting(ref field, value, propertyName);
        }

        [NotifyPropertyChangedInvocator]
        protected bool SetPropertyAffecting<T>(ref T field, T value, params string[] propertyNames)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;

            foreach (var propertyName in propertyNames)
            {
                this.OnPropertyChanged(propertyName);
            }

            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}