
namespace AutomatonBuilder.Interfaces
{
    public interface IAutomatonMemory
    {
        public IAutomatonMemory Clone();

        public void PrintMemroy();

        public bool IsLastCharacter();

        public bool IsDetermenistic();

        public bool IsFinishableWord();
    }
}
