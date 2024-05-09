using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.Contracts.Entities;

namespace Core.Contracts.Helpers
{
    public interface IValidaciones
    {
        Task<PersonaUpdateDTO> ValidarUpdatePersona(PersonaEntity personaOriginal, PersonaEntity personaUpdate);

        Task<ClienteEntity> ValidarUpdateCliente(ClienteEntity clienteOriginal, ClienteEntity clienteUpdate);
    }
}
