using System.Threading.Tasks;

namespace Rocket.Libraries.DatabaseIntegrator
{
    public interface IReaderBase<TModel, TIdentifier> where TModel : ModelBase<TIdentifier>
    {
        Task<TModel> GetByIdAsync(TIdentifier id, bool? showDeleted);
    
    }

    public abstract class ReaderBase<TModel, TIdentifier> : IReaderBase<TModel, TIdentifier> where TModel : ModelBase<TIdentifier>
    {

        


        public ReaderBase(
            IDatabaseHelper<TIdentifier> databaseHelper)
        {
            DatabaseHelper = databaseHelper;
        }

        protected IDatabaseHelper<TIdentifier> DatabaseHelper { get; }

        public abstract Task<TModel> GetByIdAsync(TIdentifier id, bool? showDeleted);

    }
}