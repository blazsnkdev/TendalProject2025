namespace TendalProject.Common.Time
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTimeNow();
        bool ValidarFecha(DateOnly fecha);
        bool ValidarMayoriaEdad(DateOnly fechaNacimiento, int edadMinima = 18);
    }
}
