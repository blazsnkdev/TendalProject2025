namespace TendalProject.Common.Results
{
    public record Error(string Code, string Message)
    {
        public static Error Validation(string message) => new("VALIDATION", message);
        public static Error NotFound(string message = "Recurso no encontrado") => new("NOT_FOUND", message);
        public static Error Unauthorized() => new("UNAUTHORIZED", "No autorizado");
        public static Error Conflict(string message) => new("CONFLICT", message);
        public static Error InternalServerError(string message = "Error interno del servidor") => new("INTERNAL", message);
    }
}
