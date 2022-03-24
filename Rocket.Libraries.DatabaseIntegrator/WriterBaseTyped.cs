namespace Rocket.Libraries.DatabaseIntegrator
{
    public class WriterBaseTyped<TModel, TIdentifier, TReader> : WriterBase<TModel, TIdentifier>
        where TModel : ModelBase<TIdentifier>
        where TReader : IReaderBase<TModel, TIdentifier>
    {
        private readonly TReader reader;

        public WriterBaseTyped(
            IDatabaseHelper<TIdentifier> databaseHelper, 
            TReader reader)
             : base(databaseHelper, reader)
        {
            this.reader = reader;
        }

        protected TReader TypedReader => reader;
    }
}