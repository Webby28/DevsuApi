namespace WebApi.Core.Contracts.Entities;

public class CuentaUpdateDTO
{
    public string TipoCuenta { get; set; } = "Ahorro";

    public string Estado { get; set; } = "Activo";
}