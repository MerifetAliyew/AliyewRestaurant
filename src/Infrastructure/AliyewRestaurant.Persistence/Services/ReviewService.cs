using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Application.DTOs.ReviewDTOs;
using AliyewRestaurant.Application.Shared;
using AliyewRestaurant.Domain.Entites;


namespace AliyewRestaurant.Persistence.Services;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<MenuItem> _menuItemRepository;

    public ReviewService(IRepository<Review> reviewRepository, IRepository<MenuItem> menuItemRepository)
    {
        _reviewRepository = reviewRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<BaseResponse<ReviewGetDto>> CreateReviewAsync(string userId, ReviewCreateDto dto)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(dto.MenuItemId);
        if (menuItem == null)
            return new BaseResponse<ReviewGetDto>("Menu item tapılmadı.", null, HttpStatusCode.NotFound);

        if (dto.Rating < 1 || dto.Rating > 5)
            return new BaseResponse<ReviewGetDto>("Rating 1-5 arasında olmalıdır.", null, HttpStatusCode.BadRequest);

        var review = new Review
        {
            MenuItemId = dto.MenuItemId,
            UserId = userId,
            CommentBody = dto.CommentBody,
            Rating = dto.Rating,
            IsConfirmed = false
        };

        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangeAsync();

        var result = new ReviewGetDto
        {
            Id = review.Id,
            UserId = review.UserId,
            CommentBody = review.CommentBody,
            Rating = review.Rating,
            IsConfirmed = review.IsConfirmed,
            ConfirmedAt = review.ConfirmedAt,
            MenuItemId = review.MenuItemId
        };

        return new BaseResponse<ReviewGetDto>("Review uğurla yaradıldı.", result, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<List<ReviewGetDto>>> GetAllReviewsAsync(bool onlyConfirmed = true)
    {
        var reviews = await _reviewRepository.GetAllAsync();

        if (onlyConfirmed)
            reviews = reviews.Where(r => r.IsConfirmed).ToList();

        var result = reviews.Select(r => new ReviewGetDto
        {
            Id = r.Id,
            UserId = r.UserId,
            CommentBody = r.CommentBody,
            Rating = r.Rating,
            IsConfirmed = r.IsConfirmed,
            ConfirmedAt = r.ConfirmedAt,
            MenuItemId = r.MenuItemId
        }).ToList();

        return new BaseResponse<List<ReviewGetDto>>("Review-lər tapıldı.", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<ReviewGetDto>> ConfirmReviewAsync(Guid reviewId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
            return new BaseResponse<ReviewGetDto>("Review tapılmadı.", null, HttpStatusCode.NotFound);

        review.IsConfirmed = true;
        review.ConfirmedAt = DateTime.UtcNow;

        _reviewRepository.Update(review);
        await _reviewRepository.SaveChangeAsync();

        var result = new ReviewGetDto
        {
            Id = review.Id,
            UserId = review.UserId,
            CommentBody = review.CommentBody,
            Rating = review.Rating,
            IsConfirmed = review.IsConfirmed,
            ConfirmedAt = review.ConfirmedAt,
            MenuItemId = review.MenuItemId
        };

        return new BaseResponse<ReviewGetDto>("Review təsdiqləndi.", result, HttpStatusCode.OK);
    }
}

