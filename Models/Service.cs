//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _ExcellOn_.Models
{
    using System;
    using System.Collections.Generic;
    
public partial class Service
{

    public Service()
    {

        this.OrderDetails = new HashSet<OrderDetail>();

        this.UserInFoes = new HashSet<UserInFo>();

    }


    public int Id { get; set; }

    public string Service_Name { get; set; }

    public Nullable<double> Service_Price { get; set; }

    public string Service_Description { get; set; }

    public Nullable<double> Service_TaxPercentage { get; set; }

    public Nullable<int> Service_SaleStatus { get; set; }

    public Nullable<double> Service_PriceSale { get; set; }



    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    public virtual ICollection<UserInFo> UserInFoes { get; set; }

}

}
