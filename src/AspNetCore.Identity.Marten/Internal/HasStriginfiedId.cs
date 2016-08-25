using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Baseline.Reflection;
using Marten;
using Marten.Linq;
using Marten.Linq.Parsing;
using Marten.Schema;

namespace AspNetCore.Identity.Marten.Internal
{
    internal class HasStringifiedId<TId> : IMethodCallParser
    {
        private static readonly PropertyInfo _property = ReflectionHelper.GetProperty<IdentityUser<TId>>(x => x.Id);

        public bool Matches(MethodCallExpression expression)
        {
            return expression.Method.Name == nameof(IdentityUserExtensions.HasStringifiedId);
        }

        public IWhereFragment Parse(IQueryableDocument mapping, ISerializer serializer, MethodCallExpression expression)
        {
            var locator = mapping.FieldFor(new MemberInfo[] { _property }).SqlLocator;
            return new WhereFragment($"{locator} = '{expression.Arguments[1].Value()}'");
        }
    }
}
