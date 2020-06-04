using System;
using System.Globalization;
using System.Linq.Expressions;
using Rocket.Libraries.PropertyNameResolver;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Extensions
    {
        private const ushort LongestPage = ushort.MaxValue;

        public static IQueryBuilder<TIdentifier> ApplyPaging<TModel, TIdentifier>(this IQueryBuilder<TIdentifier> queryBuilder, TypedPropertyNamedResolver<TModel> field, int? page, ushort? pageSize)
        {
            var safePage = GetSafePage(page);
            var safePageSize = GetSafePageSize(pageSize);
            queryBuilder.ApplyPaging<TModel>(field, safePage, safePageSize);
            return queryBuilder;
        }

        public static IQueryBuilder<TIdentifier> SetDeletedRecordsInclusionState<TModel,TIdentifier>(this IQueryBuilder<TIdentifier> queryBuilder, bool? showDeleted)
            where TModel : ModelBase<TIdentifier>
        {
            if (showDeleted == null || showDeleted.Value == false)
            {
                queryBuilder.ManageDeletedRecordsVisibility<TModel>(false);
            }
            else
            {
                queryBuilder.ManageDeletedRecordsVisibility<TModel>(true);
            }

            return queryBuilder;
        }

        private static uint GetSafePage(int? page)
        {
            if (page == null)
            {
                return 1;
            }
            else if (page < 1)
            {
                throw new Exception($"Page number '{page}' is invalid. The smallest allowable page number is '1'");
            }
            else
            {
                return uint.Parse(page.Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            }
        }

        private static ushort GetSafePageSize(ushort? pageSize)
        {
            if (pageSize == null)
            {
                return LongestPage;
            }
            else
            {
                return pageSize.Value;
            }
        }
    }
}