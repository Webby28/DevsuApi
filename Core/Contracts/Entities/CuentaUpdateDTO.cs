namespace WebApi.Core.Contracts.Entities;

public class CuentaUpdateDto
{
    public string TipoCuenta { get; set; } = "Ahorro";

    public char Estado { get; set; } = 'A';
}