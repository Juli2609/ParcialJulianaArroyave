using ParcialJulianaArroyave.DAL.Entities;

namespace ParcialJulianaArroyave.DAL
{
    public class SeederDb
    {
        private readonly DataBaseContext _context;
        public SeederDb(DataBaseContext context)
        {
            _context = context;
        }

        public async Task SeederAsync()
        {
            await _context.Database.EnsureCreatedAsync(); 
            await PopulateTicketsAsync();

            
        }

        private async Task PopulateTicketsAsync()
        {
            if (!_context.Tickets.Any())
            {
                for (int i = 1; i <= 50; i++)
                {
                    _context.Tickets.Add(new Ticket
                    {
                        UseDate = null,
                        IsUsed = false,
                        EntranceGate = null
                    });
                }

                await _context.SaveChangesAsync();
            }

        }
    }
}
