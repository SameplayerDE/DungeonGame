namespace QColonFrame
{
    public partial class Entity
    {
        public Guid Id { get; private set; }
        private readonly Dictionary<Type, Component> _components;

        public Entity()
        {
            Id = Guid.NewGuid();
            _components = new Dictionary<Type, Component>();
        }

        public void AddComponent(Component component)
        {
            var type = component.GetType();
            if (!_components.ContainsKey(type))
            {
                _components[type] = component;
            }
        }

        public Component? GetComponent(Type type)
        {
            if (_components.TryGetValue(type, out Component component))
            {
                return component;
            }

            return null;
        }

        public T? GetComponent<T>() where T : Component
        {
            return GetComponent(typeof(T)) as T;
        }
    }
}
