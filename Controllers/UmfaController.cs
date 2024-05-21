﻿using ClientPortal.Controllers.Authorization;
using ClientPortal.Models.RequestModels;
using ClientPortal.Models.ResponseModels;
using ClientPortal.Services;

namespace ClientPortal.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UmfaController : ControllerBase
    {
        private readonly ILogger<UmfaController> _logger;
        private readonly IUmfaService _umfaService;
        public UmfaController(ILogger<UmfaController> logger, IUmfaService umfaService) 
        {
            _logger = logger;
            _umfaService = umfaService;
        }

        [HttpGet("shops")]
        public async Task<ActionResult<List<UMFAShop>>> GetUmfaShops([FromQuery] UmfaShopsRequest request)
        {
            try
            {
                var response = await _umfaService.GetShopsAsync(request);
                
                return response.Shops;
            }
            catch (Exception e) 
            {
                _logger.LogError(e, "Could not get shops from umfa");
                return Problem(e.Message);
            }
        }

        [HttpGet("tenants")]
        public async Task<ActionResult<List<UMFATenant>>> GetUmfaTenants([FromQuery] UmfaTenantsSpRequest request)
        {
            try
            {
                return await _umfaService.GetTenantsAsync(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not get shops from umfa");
                return Problem(e.Message);
            }
        }

        [HttpGet("tenant-shops")]
        public async Task<ActionResult<List<UMFAShop>>> GetUmfaTenantShops([FromQuery] UmfaTenantShopsSpRequest request)
        {
            try
            {
                return await _umfaService.GetTenantShopsAsync(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not get shops from umfa");
                return Problem(e.Message);
            }
        }
    }
}
