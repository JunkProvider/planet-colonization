namespace SpaceLogistic.WpfView.ViewModel
{
    using System.Linq;

    using SpaceLogistic.Core.Model;

    public sealed class StorageViewModel : ViewModelBase
    {
        private readonly Storage storage;

        private string content;

        public StorageViewModel(Storage storage)
        {
            this.storage = storage;
        }

        public string Content
        {
            get => this.content;
            private set
            {
                if (value == this.content)
                {
                    return;
                }

                this.content = value;
                this.OnPropertyChanged();
            }
        }

        public void Update()
        {
            this.Content = string.Join(", ", this.storage.Items.Select(i => $"{i.Amount}t {i.Name}"));
        }
    }
}
