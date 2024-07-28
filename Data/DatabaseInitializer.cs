using OrderManagement.Models;

namespace OrderManagement.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!context.Orders.Any())
                {
                    var orders = new List<Order>
                {
                    new Order
        {
            OrderNo = "ORD001",
            OrderDate = DateTime.Parse("2024-07-25"),
            DeptCode = "DEP001",
            CustCode = "CUST001",
            EmpCode = "EMP001",
            RequiredDate = "2024-07-30",
            CustOrderNo = "CUST_ORDER_001",
            WhCode = "WH001",
            CmpTax = 10,
            SlipComment = "Urgent delivery needed",
            UpdateDate = DateTime.Now,
            Updater = "admin",
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    OrderNo = "ORD001",
                    SoRowNo = 1,
                    ProdCode = "PROD001",
                    ProdName = "Product 1",
                    UnitPrice = 100,
                    Quantity = 10,
                    CmpTaxRate = 10,
                    ReserveQty = 0,
                    DeliveryOrderQty = 0,
                    DeliveredQty = 0,
                    CompleteFlg = 0,
                    Discount = 0,
                    DeliveryDate = "2024-07-30",
                    UpdateDate = DateTime.Now,
                    Updater = "admin"
                },
                new OrderDetail
                {
                    OrderNo = "ORD001",
                    SoRowNo = 2,
                    ProdCode = "PROD002",
                    ProdName = "Product 2",
                    UnitPrice = 150,
                    Quantity = 5,
                    CmpTaxRate = 10,
                    ReserveQty = 0,
                    DeliveryOrderQty = 0,
                    DeliveredQty = 0,
                    CompleteFlg = 0,
                    Discount = 0,
                    DeliveryDate = "2024-07-30",
                    UpdateDate = DateTime.Now,
                    Updater = "admin"
                }
            }
        },
                    new Order
        {
            OrderNo = "ORD002",
            OrderDate = DateTime.Parse("2024-07-24"),
            DeptCode = "DEP002",
            CustCode = "CUST002",
            EmpCode = "EMP002",
            RequiredDate = "2024-07-29",
            CustOrderNo = "CUST_ORDER_002",
            WhCode = "WH002",
            CmpTax = 8,
            SlipComment = "Standard delivery",
            UpdateDate = DateTime.Now,
            Updater = "admin",
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    OrderNo = "ORD002",
                    SoRowNo = 1,
                    ProdCode = "PROD003",
                    ProdName = "Product 3",
                    UnitPrice = 80,
                    Quantity = 8,
                    CmpTaxRate = 8,
                    ReserveQty = 0,
                    DeliveryOrderQty = 0,
                    DeliveredQty = 0,
                    CompleteFlg = 0,
                    Discount = 0,
                    DeliveryDate = "2024-07-29",
                    UpdateDate = DateTime.Now,
                    Updater = "admin"
                }
            }
        }
                };
                    context.Orders.AddRange(orders);
                    context.SaveChanges();
                }
               
            }
        }
    }
}

