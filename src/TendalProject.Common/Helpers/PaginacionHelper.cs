using TendalProject.Common.Utils;

namespace TendalProject.Common.Helpers
{
    public static class PaginacionHelper
    {
        public static PaginaResult<T> Paginacion<T>(IEnumerable<T> lista, int indicePagina, int tamanioPagina)
        {
            var total = lista.Count();
            var items = lista.Skip((indicePagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToList();

            var resultadoPaginacion = new PaginaResult<T>()
            { 
                Items = items,
                TotalRegistros = total,
                PaginaIndice = indicePagina,
                TamanioPagina = tamanioPagina
            };
            return resultadoPaginacion;
        }
    }
}
