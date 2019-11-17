using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using APIIndicadores.Models;
using Dapper;

namespace APIIndicadores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndicadoresController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Indicador> Get(
            [FromServices]IConfiguration configuration)
        {
            using (SqlConnection conexao = new SqlConnection(
                configuration.GetConnectionString("BaseIndicadores")))
            {
                return conexao.Query<Indicador>(
                    "SELECT * FROM dbo.Indicadores");
            }
        }    

        [HttpGet("{indicador}")]
        public ActionResult<Indicador> Get(
            [FromServices]IConfiguration configuration,
            string indicador)
        {
            Indicador resultado = null;

            using (SqlConnection conexao = new SqlConnection(
                configuration.GetConnectionString("BaseIndicadores")))
            {
                resultado = conexao.QueryFirstOrDefault<Indicador>(
                    "SELECT * FROM dbo.Indicadores " +
                    "WHERE Sigla = @siglaIndicador",
                    new { siglaIndicador = indicador });
            }

            if (resultado != null)
                return resultado;
            else
            {
                return NotFound(
                    new {
                            Mensagem = "Indicador inválido ou inexistente."
                        });
            }
        }    
    }
}