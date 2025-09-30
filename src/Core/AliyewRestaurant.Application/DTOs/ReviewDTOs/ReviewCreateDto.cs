using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliyewRestaurant.Application.DTOs.ReviewDTOs;

public class ReviewCreateDto
{
    public Guid MenuItemId { get; set; }
    public string CommentBody { get; set; } // İstifadəçinin şərhi / təklifi
    public int Rating { get; set; } // 1-5
}