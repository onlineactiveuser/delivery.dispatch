using FluentValidation.Results;


namespace Application.Abstraction.Messaging
{
    public abstract class Command<T> : ICommand<T>
    {
        public DateTime Timestamp { get; protected set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public abstract bool IsValid();
    }
}