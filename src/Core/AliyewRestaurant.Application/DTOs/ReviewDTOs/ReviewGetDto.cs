using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliyewRestaurant.Application.DTOs.ReviewDTOs;

public class ReviewGetDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string CommentBody { get; set; }
    public int Rating { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public Guid MenuItemId { get; set; }
}