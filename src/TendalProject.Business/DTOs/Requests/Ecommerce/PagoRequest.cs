using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TendalProject.Business.DTOs.Requests.Ecommerce
{
    public record PagoRequest
    (
        Guid ClienteId,
        decimal Total
    );
}
