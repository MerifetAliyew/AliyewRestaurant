using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.CategoryDTOs;
using AliyewRestaurant.Application.Shared;
using System.Net;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliyewRestaurant.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<List<CategoryGetDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAllAsync();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }


    [HttpGet("tree")]
    [ProducesResponseType(typeof(BaseResponse<List<CategoryGetDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTree()
    {
        var result = await _categoryService.GetTreeAsync();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        var result = await _categoryService.CreateCategoryAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(CategoryUpdateDto dto)
    {
        var result = await _categoryService.UpdateCategoryAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

}
