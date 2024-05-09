using System.Runtime.Serialization;
using WebApi.Core.Contracts.Enums;

namespace WebApi.Core.Contracts.Helpers
{
    [Serializable]
    public class ReglaNegociosException : Exception
    {
        public string? Mensaje { get; } = string.Empty;
        public ErrorType CodigoError { get; set; }

        // Constructor por defecto
        public ReglaNegociosException()
        {
        }

        // Constructor con mensaje
        public ReglaNegociosException(string mensaje)
            : base(mensaje)
        {
            this.Mensaje = mensaje;
        }

        // Constructor con mensaje y código de error
        public ReglaNegociosException(string mensaje, ErrorType codigoError)
            : this(mensaje)
        {
            this.CodigoError = codigoError;
        }

        // Constructor con mensaje y excepción interna
        public ReglaNegociosException(string mensaje, Exception innerException)
            : base(mensaje, innerException)
        {
            this.Mensaje = mensaje;
        }

        // Constructor con información de serialización
        protected ReglaNegociosException(SerializationInfo info, StreamingContext context)
    : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            this.Mensaje = info.GetString("Mensaje") ?? "Un mensaje de error predeterminado";

            // Comprueba si el valor para "CodigoError" es DBNull o no está presente.
            if (info.GetValue("CodigoError", typeof(ErrorType)) is ErrorType code)
            {
                this.CodigoError = code;
            }
            else
            {
                // Asigna un valor predeterminado o lanza una excepción.
                this.CodigoError = ErrorType.ERROR_INTERNO; // Asigna valor de error por defecto.
            }
        }

        // Método para serializar datos
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("Mensaje", this.Mensaje);
            info.AddValue("CodigoError", this.CodigoError);

            base.GetObjectData(info, context);
        }
    }
}