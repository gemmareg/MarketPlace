namespace MarketPlace.Shared
{
    public static class ErrorMessages
    {
        // CartItem errors
        public const string INVALID_USER = "El usuario no es válido";
        public const string INVALID_PRODUCT = "El producto no es válido";
        public const string INVALID_QUANTITY = "La cantidad no es válida";

        // Category errors
        public const string INVALID_CATEGORY_NAME = "El nombre de la categoría no puede estar vacío.";

        // Order errors
        public const string INVALID_USER_FOR_ORDER = "El usuario no puede ser nulo";
        public const string EMPTY_CART_ITEMS = "La lista del carrito no puede estar vacía.";
        public const string ERROR_CREATING_ORDER_ITEM = "Error al crear OrderItem desde CartItem.";
        public const string ORDER_CANNOT_BE_PAID = "La orden ya ha sido pagada.";

        // OrderItem errors
        public const string INVALID_ORDER_FOR_ORDER_ITEM = "El pedido no puede ser nulo";
        public const string INVALID_CART_ITEM_FOR_ORDER_ITEM = "El item del carrito no puede ser nulo.";

        // Product errors
        public const string INVALID_CATEGORY_FOR_PRODUCT = "El producto debe estar asignado a una categoría.";
        public const string INVALID_SELLER_FOR_PRODUCT = "Es necesario asignar un vendedor.";
        public const string INVALID_PRODUCT_NAME = "El nombre del producto no puede estar vacío.";
        public const string INVALID_PRODUCT_PRICE = "El precio del producto no puede ser negativo.";
        public const string INVALID_PRODUCT_STOCK = "El stock del producto no puede ser negativo.";

        // Review errors
        public const string INVALID_USER_FOR_REVIEW = "La reseña debe estar asociada a un usuario.";
        public const string INVALID_PRODUCT_FOR_REVIEW = "La reseña debe estar asociada a un producto.";
        public const string INVALID_RATING_FOR_REVIEW = "La calificación debe estar entre 1 y 5.";

        // User errors
        public const string INVALID_USER_NAME = "El nombre del usuario no puede estar vacío.";
        public const string INVALID_USER_ID = "La Id no es válida.";

        // Payment errors
        public const string INVALID_ORDER_FOR_PAYMENT = "El pedido no puede ser nulo para crear un pago.";
        public const string INVALID_PAYMENT_AMOUNT = "El monto del pago no puede ser negativo.";
        public const string INVALID_PAYMENT_METHOD = "El método de pago no es válido.";
    }
}
