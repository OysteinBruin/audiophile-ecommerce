namespace Catalog.Core.Constants;

public static class StrapiApiConstants
{
    public static class ApiResponse
    {
        public static string DataRoot = "data";
        public static string PaginationRoot = "meta";
    }
    
    public static class ApiRequest
    {
        public static string ProductEndpointBaseUrl = "api/products/";
        public static string GetProductsListQuery = "?fields[0]=name&fields[1]=description&fields[2]=updatedAt&populate=image&filters[category][$eq]=";
        public static string GetProductsDetailQuery = "?populate=image&populate=gallery";
        public static string[] ProductCategoryTypes = {"speaker", "earphone", "headphone"};
    }
}