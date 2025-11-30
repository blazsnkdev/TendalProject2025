namespace TendalProject.Common.Time
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTimeNow() => DateTime.Now;

        public bool ValidarFecha(DateOnly fecha) => fecha <= DateOnly.FromDateTime(DateTime.UtcNow);

        public bool ValidarMayoriaEdad(DateOnly fechaNacimiento, int edadMinima = 18)
        {
            var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
            int edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento > hoy.AddYears(-edad))
            {
                edad--;
            }
            return edad >= edadMinima;
        }
    }
}
