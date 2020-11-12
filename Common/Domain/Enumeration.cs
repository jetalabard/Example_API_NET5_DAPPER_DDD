using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Domain
{
    public record Enumeration
    {
        public int Id { get; }
        public string Code { get; }

        public Enumeration(int id, string code) => (Id, Code) = (id, code);

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }
    }
}
