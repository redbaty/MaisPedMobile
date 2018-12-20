using System.Collections.Generic;

namespace MaisPedMobile.SyncServer.Models
{
    public class Enterprise
    {
        public int Id { get; set; }

        public string Cnpj { get; set; }

        public string TerminalKey { get; set; }

        public ICollection<Salesman> Salesman { get; set; }
    }
}