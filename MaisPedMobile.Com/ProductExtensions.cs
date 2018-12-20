using SGE32.DAL.EnterpriseModels;

namespace MaisPedMobile.Com
{
    public static class ProductExtensions
    {
        public static Product ToProduct(this ProdutosEmp emp, Produtos prod, string barcode = null)
        {
            return new Product
            {
                Id = emp.Codigoproduto,
                FullPrice = emp.Prvendvistaproduto,
                Name = prod.Descricproduto,
                Barcode = barcode
            };
        }
    }
}