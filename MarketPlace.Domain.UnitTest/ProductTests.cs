using MarketPlace.Shared;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Domain.UnitTest
{
    public class ProductTests
    {
        private readonly Category _category;
        private readonly User _seller;
        private readonly string _name;
        private readonly string _description;
        public ProductTests()
        {
            //TODO: finish the constructor with the necessary data
            _category = Category.Create("Lorem", "Ipsum").Data!;
            _seller = User.Create(Guid.NewGuid(), "Lorem User").Data!;



            _name = "Cacahuetes Tostados Naturales – 1 kg";
            _description = @"Cacahuetes tostados de alta calidad, seleccionados y tostados de forma uniforme para ofrecer un sabor natural y una textura crujiente. Sin sal añadida ni conservantes, ideales para quienes buscan un snack sencillo y versátil.

Aptos para consumir solos o como ingrediente en ensaladas, platos salteados, repostería o para preparar mantequilla de cacahuete casera. Presentados en bolsa resellable para conservar mejor su frescura.

Un básico práctico y sabroso para el día a día.";
        }

        [Theory]
        [InlineData(10.0, 100, Product.ProductState.Active)]
        public void CreateShouldReturnExpectedResult(
            decimal price,
            int stock,
            Product.ProductState state)
        {
            //TODO: implement the method to run the tests

            //Act
            var product = Product.Create(_category, _seller, _name, _description, price, stock, DateTime.Today.AddDays(-1), state);
        }
    }
}
