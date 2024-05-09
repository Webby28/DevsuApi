namespace Core.Contracts.Helpers
{
    public class JsonPatchOperation
    {
        public string Op { get; set; } // Operación (add, remove, replace, etc.)
        public string Path { get; set; } // Ruta del documento donde se aplicará la operación
        public object Value { get; set; } // Valor de la operación (opcional)
    }
}