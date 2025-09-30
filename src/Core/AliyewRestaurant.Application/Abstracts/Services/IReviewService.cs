using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliyewRestaurant.Application.DTOs.ReviewDTOs;
using AliyewRestaurant.Application.Shared;

namespace AliyewRestaurant.Application.Abstracts.Services;

public interface IReviewService
{
    Task<BaseResponse<ReviewGetDto>> CreateReviewAsync(string userId, ReviewCreateDto dto);
    Task<BaseResponse<List<ReviewGetDto>>> GetAllReviewsAsync(bool onlyConfirmed = true);
    Task<BaseResponse<ReviewGetDto>> ConfirmReviewAsync(Guid reviewId);
}