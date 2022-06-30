namespace Farsica.Framework.Data
{
    public class ParameterDto : ParameterDto<int>
    {
    }

    public class ParameterDto<T>
    {
        public T Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }

        public bool Disabled { get; set; }

        public string Value { get; set; }

        public string Group { get; set; }
    }
}
