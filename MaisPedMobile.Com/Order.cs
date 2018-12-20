using System.Collections.Generic;

namespace MaisPedMobile.Com
{
    public class Order
    {
        public Person Buyer { get; set; }
        public ICollection<OrderProduct> Products { get; set; }
    }

    public class OrderProduct : Product
    {
        public int Quantity { get; set; }

        public decimal Discount { get; set; }
    }
}