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
            Cancelled
        }

        public enum PaymentMethod
        {
            Undefined,
            CreditCard,
            DebitCard,
            PayPal,
            BankTransfer
        }

        public enum ProductState
        {
            Active,
            Inactive,
            SoldOut
        }
    }
}
