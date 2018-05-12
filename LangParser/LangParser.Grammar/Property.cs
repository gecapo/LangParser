namespace LangParser.Grammar
{
    class Property
    {
        public Property(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }


}
