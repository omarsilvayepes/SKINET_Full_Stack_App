using Core.Entities;

namespace Core.Specifications
{
    public class ProductSpecification:BaseSpecification<Product>
    {

        //Pass to the BaseSpecification class the specifications product parameter brand and type
        public ProductSpecification(string? brand,string? type,string? sort):base(x=>

            (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&
            (string.IsNullOrWhiteSpace(type)  || x.Type == type))
        {
            switch (sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => x.Price);
                    break;
                default:
                    AddOrderBy(x=> x.Name); 
                    break;
            }
        }
    }
}
