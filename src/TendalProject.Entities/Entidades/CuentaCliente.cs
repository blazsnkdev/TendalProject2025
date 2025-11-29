using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TendalProject.Entities.Entidades
{
    public class CuentaCliente
    {
        public int ClienteId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Cliente Cliente { get; set; }
    }
}
