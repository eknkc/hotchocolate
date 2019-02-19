using System.Collections.Generic;

namespace HotChocolate.Stitching.Merge.Handlers
{
    internal static class TypeMergeHelpers
    {
        public static NameString CreateName<T>(
            ISchemaMergeContext context,
            params T[] types)
            where T : ITypeInfo =>
            CreateName(context, (IReadOnlyList<T>)types);


        public static NameString CreateName<T>(
            ISchemaMergeContext context,
            IReadOnlyList<T> types)
            where T : ITypeInfo
        {
            NameString name = types[0].Definition.Name.Value;

            if (context.ContainsType(name))
            {
                for (int i = 0; i < types.Count; i++)
                {
                    name = types[i].CreateUniqueName();
                    if (!context.ContainsType(name))
                    {
                        break;
                    }
                }

                if (context.ContainsType(name))
                {
                    name = types[0].Definition.Name.Value;

                    for (int i = 0; i < 10000; i++)
                    {
                        NameString n = name + $"_{i}";
                        if (!context.ContainsType(name))
                        {
                            name = n;
                            break;
                        }
                    }
                }
            }

            return name;
        }
    }
}
