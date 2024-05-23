using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string CompanyDelegate { get; set; } = null!;

    public string CompanyInn { get; set; } = null!;

    public string CompanyAddress { get; set; } = null!;

    public string? CompanyEmailAddress { get; set; }

    public string? CompanyPhone { get; set; }

    public string CompanyLogin { get; set; } = null!;

    public string CompanyPassword { get; set; } = null!;

    public virtual ICollection<Agreement> Agreements { get; set; } = new List<Agreement>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public string Full
    {
        get
        {
            return $"{CompanyName} {CompanyDelegate} {CompanyInn} {CompanyAddress} {CompanyLogin} {CompanyPassword}";
        }
    }
}

