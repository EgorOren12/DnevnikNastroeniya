using DnevnikNastroeniya.Data;
using DnevnikNastroeniya.Models;

namespace DnevnikNastroeniya.Commands
{
    public class AddRecordCommand : ICommand
    {
        private readonly Record _record;

        public AddRecordCommand(Record record)
        {
            _record = record;
        }

        public void Execute()
        {
            using (var context = new AppDbContext())
            {
                context.Records.Add(_record);
                context.SaveChanges();
            }
        }
    }
}