namespace TendalProject.Common.Results
{
    public record Error(string Code, string Message)
    {
        public static Error Validation(string message) =>
            new("ERROR_VALIDATION", message);
        public static Error NotFound(string message = "Recurso no encontrado") =>
            new("ERROR_NOT_FOUND", message);
        public static Error Unauthorized(string message = "No autorizado") =>
            new("ERROR_UNAUTHORIZED", message);
        public static Error Conflict(string message) =>
            new("ERROR_CONFLICT", message);
        public static Error Internal(string message = "Error interno del servidor") =>
            new("ERROR_INTERNAL", message);        
        public static Error Database(string message = "Error de base de datos") =>
            new("ERROR_DATABASE", message);
    }
}
