namespace MarketPlace.Shared
{
    public class Enums
    {
        public enum UserRole
        {
            Buyer,
            Seller,
            Admin
        }

        public enum PaymentState
        {
            Pending,
            Completed,
            Failed
        }

        public enum OrderStatus
        {
            Pending,
            Paid,
            Sent,
            Delivered,
            Canceled
        }
    }
}
