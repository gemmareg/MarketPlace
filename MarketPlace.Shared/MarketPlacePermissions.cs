namespace MarketPlace.Shared
{
    public static class MarketPlacePermissions
    {
        public const string CategoriesCreate = "categories:create";
        public const string CategoriesUpdate = "categories:update";
        public const string CategoriesDelete = "categories:delete";

        public const string ProductsUpdateAny = "products:update:any";
        public const string ProductsDeleteAny = "products:delete:any";

        public const string OrdersReadAny = "orders:read:any";
    }
}
