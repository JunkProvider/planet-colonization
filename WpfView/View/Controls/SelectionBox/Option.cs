namespace SpaceLogistic.WpfView.View.SelectionBox
{
    public sealed class Option<TValue> : IOption
    {
        public Option(TValue value)
            : this(value?.ToString() ?? string.Empty, value)
        {
        }

        public Option(string name, TValue value)
        {
            this.Name = name;
            this.Value = value;
        }
        
        public string Name { get; set; }

        public TValue Value { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
