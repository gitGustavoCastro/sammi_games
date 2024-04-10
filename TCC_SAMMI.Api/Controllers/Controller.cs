using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx.Expr;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel;
using static Mysqlx.Expect.Open.Types;

namespace TCC_SAMMI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioService _userService;
        public UsuarioController(IUsuarioService bookService)
        {
            _userService = bookService;
        }

        /*
        200 OK
        201 Created
        204 No Content
        400 Bad Request
        401 Unauthorized
        403 Forbidden
        404 Not Found
        405 Method Not Allowed
        415 Unsuported Media Type
        500 Internal Server Error
        502 Bad Gateway
        503 Service Unavailable
        */

        /// <summary>Retorna uma lista com todos os usuarios cadastrados</summary>
        /// <remarks>Faça uma consulta diretamente no banco de dados!!</remarks>
        [HttpGet]
        [Tags("Usuario")]
        public IActionResult returnUsuarios()
        {
            var users = _userService.returnUsuarios();
            return Ok(users);
        }

        /// <summary>Retorna um unico usuario baseado em seu ID</summary>
        /// <remarks>Faça uma consulta especifica diretamente no banco de dados!!</remarks>
        /// <example>5</example>
        [HttpGet("{id:int}")]
        [Tags("Usuario")]
        public IActionResult returnUsuario(int id)
        {
            var user = _userService.returnUsuario(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>Retorna um unico usuario baseado em seu Nome</summary>
        /// <remarks>Faça uma consulta especifica diretamente no banco de dados!!</remarks>
        /// <example>Teste</example>
        [HttpGet("{nome}")]
        [Tags("Usuario")]
        public IActionResult returnUsuariobyNome(string nome)
        {
            var user = _userService.returnUsuariobyNome(nome);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>Cadastra um novo usuario</summary>
        /// <remarks>Faça uma inserção diretamente no banco de dados!</remarks>
        /// <response code="200">OK!</response>
        /// <response code="201">Usuario criado com sucesso!</response>
        /// <example>5</example>
        [HttpPost]
        [Tags("Usuario")]
        public IActionResult addUsuario(Usuario user)
        {
            if (user == null)
            {
                return BadRequest("Invalid data received");
            }
            else
            {
                bool result = _userService.addUsuario(user);
                return Ok(result);
            }
        }

        /// <summary>Edite um usuario baseado em eu ID</summary>
        /// <remarks>Faça uma edição diretamente no banco de dados!!</remarks>
        /// <example>10</example>
        [HttpPut("{email}")]
        [Tags("Usuario")]
        public IActionResult updateUsuario(string email, Usuario user)
        {
            var editeduser = _userService.updateUsuario(email, user);
            return Ok(editeduser);
        }

        /// <summary>Remova um usuario baseado em seu ID</summary>
        /// <remarks>Faça uma exclusão diretamente no banco de dados!!</remarks>
        /// <example>10</example>
        [HttpDelete("{id}")]
        [Tags("Usuario")]
        public IActionResult deleteUsuario(int id)
        {
            _userService.deleteUsuario(id);
            return NoContent();
        }
    }
}
