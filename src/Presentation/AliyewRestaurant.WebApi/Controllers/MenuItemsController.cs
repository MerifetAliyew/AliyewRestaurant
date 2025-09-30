using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.MenuItemDTOs;
using AliyewRestaurant.Application.Shared;
using System.Net;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliyewRestaurant.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuItemController : ControllerBase
    {
       private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // ✅ Menyu elementləri siyahısı
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<MenuItemGetDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _menuItemService.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        // ✅ ID ilə menyu elementini gətir
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BaseResponse<MenuItemGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<MenuItemGetDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _menuItemService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        // ✅ Menyu elementini yarat
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<MenuItemGetDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<MenuItemGetDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] MenuItemCreateDto dto)
        {
            var result = await _menuItemService.CreateMenuItemAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        // ✅ Menyu elementini yenilə
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<MenuItemGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<MenuItemGetDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromBody] MenuItemUpdateDto dto)
        {
            var result = await _menuItemService.UpdateMenuItemAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        // ✅ Menyu elementini sil
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _menuItemService.DeleteMenuItemAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }  

    }

