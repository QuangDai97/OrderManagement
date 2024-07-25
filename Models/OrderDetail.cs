namespace OrderManagement.Models;

public class OrderDetail
{
    public string OrderNo { get; set; }
    public int SoRowNo { get; set; }
    public string ProdCode { get; set; }
    public string ProdName { get; set; }
    public int UnitPrice { get; set; }
    public int Quantity { get; set; }
    public int? CmpTaxRate { get; set; }
    public int ReserveQty { get; set; }
    public int DeliveryOrderQty { get; set; }
    public int DeliveredQty { get; set; }
    public int CompleteFlg { get; set; }
    public int Discount { get; set; }
    public string? DeliveryDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string? Updater { get; set; }
    public Order Order { get; set; }
}