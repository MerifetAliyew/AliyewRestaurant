using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliyewRestaurant.Domain.Entites;

public class VIPMembership : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalSpent { get; set; } // Ay ərzində xərclənən məbləğ
}