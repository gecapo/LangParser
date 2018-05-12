namespace LangParser.Grammar
{
    class Entity
    {
        //TODO: Fix model


        public Entity(string scheme, Property[] parameters)
        {
            Name = scheme;
            Properties = parameters;
        }

        public string Name { get; }
        public Property[] Properties { get; }
    }


}
