using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TripsEntityFramework.Interfaces;

namespace TripsEntityFramework.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id, CancellationToken cancellationToken)
        {
            bool result = await _clientService.DeleteClientAsync(id, cancellationToken);
            if (!result)
            {
                return BadRequest(new ApiResponse<string>(false, "Cannot delete client. Either the client does not exist or the client is assigned to one or more trips.", null));
            }
            return Ok(new ApiResponse<string>(true, "Client deleted successfully.", null));
        }
    }
}