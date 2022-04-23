using System.Windows.Controls;

namespace AutomatonBuilder.Interfaces
{
    public interface IAutomatonMemory
    {
        public IAutomatonMemory Clone();

        public void PrintMemroy(Canvas memoryCanvas);

        public bool IsLastCharacter();

        public bool IsDetermenistic();

        public bool IsFinishableWord();
    }
}
