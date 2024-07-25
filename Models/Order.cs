namespace OrderManagement.Models;

public class Order
{
    public string OrderNo { get; set; }
    public DateTime OrderDate { get; set; }
    public string DeptCode { get; set; }
    public string CustCode { get; set; }
    public int? CustSubNo { get; set; }
    public string EmpCode { get; set; }
    public string? RequiredDate { get; set; }
    public string? CustOrderNo { get; set; }
    public string WhCode { get; set; }
    public int CmpTax { get; set; }
    public string? SlipComment { get; set; }
    public DateTime UpdateDate { get; set; }
    public string? Updater { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}