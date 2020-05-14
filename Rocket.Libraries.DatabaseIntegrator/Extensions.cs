using System;
using System.Globalization;
using System.Linq.Expressions;
using Rocket.Libraries.Qurious;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public static class Extensions
    {
        private const ushort LongestPage = ushort.MaxValue;

        public static QBuilder ApplyPaging<TTable, TField>(this QBuilder qBuilder, Expression<Func<TTable, TField>> fieldDescriber, int? page, ushort? pageSize)
        {
            var safePage = GetSafePage(page);
            var safePageSize = GetSafePageSize(pageSize);
            qBuilder.UseSqlServerPagingBuilder<TTable>()
                    .PageBy(fieldDescriber, safePage, safePageSize);
            return qBuilder;
        }

        public static QBuilder SetDeletedRecordsInclusionState<TTable,TIdentifier>(this QBuilder qBuilder, bool? showDeleted)
            where TTable : ModelBase<TIdentifier>
        {
            if (showDeleted == null || showDeleted.Value == false)
            {
                qBuilder
                    .UseTableBoundFilter<TTable>()
                    .WhereEqualTo(table => table.Deleted, false);
            }

            return qBuilder;
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