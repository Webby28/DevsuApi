using Core.Contracts.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Interfaces;

namespace Core.Contracts.Validators
{
    public class Validaciones : IValidaciones
    {
        private readonly ILogger<Validaciones> _logger;
        private readonly IConfiguration _configuration;
        private readonly IClientePersonaRepository _clientePersonaRepository;
        public Validaciones(ILogger<Validaciones> logger, IConfiguration configuration, IClientePersonaRepository clientePersonaRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _clientePersonaRepository = clientePersonaRepository;
        }

        public Task<ClienteEntity> ValidarUpdateCliente(ClienteEntity clienteOriginal, ClienteEntity clienteUpdate)
        {
            throw new NotImplementedException();
        }

        public Task<PersonaUpdateDTO> ValidarUpdatePersona(PersonaEntity personaOriginal, PersonaEntity personaUpdate)
        {
            var updateDTO = new PersonaUpdateDTO();


            if (!string.IsNullOrWhiteSpace(personaUpdate.Nombre) && !string.IsNullOrWhiteSpace(personaOriginal.Nombre))
            {
                if(personaOriginal.Nombre != personaUpdate.Nombre)
                    updateDTO.Nombre = personaUpdate.Nombre;
            }

            if (!string.IsNullOrWhiteSpace(personaUpdate.Genero) && !string.IsNullOrWhiteSpace(personaOriginal.Genero))
            {
                if (personaOriginal.Genero != personaUpdate.Genero)
                    updateDTO.Genero = personaUpdate.Genero;
            }

            if (!string.IsNullOrWhiteSpace(personaUpdate.Identificacion) && !string.IsNullOrWhiteSpace(personaOriginal.Identificacion))
            {
                if (personaOriginal.Identificacion != personaUpdate.Identificacion)
                    updateDTO.Identificacion = personaUpdate.Identificacion;
            }
            
            if (personaUpdate.Edad > 0)
            {
                if(personaUpdate.Edad != personaOriginal.Edad)
                    updateDTO.Edad = personaUpdate.Edad;
            }
            else
            {
                throw new ReglaNegociosException("Ingrese una edad válida.", ErrorType.EDAD_INVALIDO);
            }
            if (!string.IsNullOrWhiteSpace(personaUpdate.Direccion))
            {
                if (personaOriginal.Direccion != personaUpdate.Direccion)
                    updateDTO.Direccion = personaUpdate.Direccion;
            }
            if (!string.IsNullOrWhiteSpace(personaUpdate.Telefono))
            {
                if (personaOriginal.Telefono != personaUpdate.Telefono)
                    updateDTO.Telefono = personaUpdate.Telefono;
            }
            return Task.FromResult(updateDTO);
        }
    }
}
