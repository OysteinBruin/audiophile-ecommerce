using Catalog.Core.Models;
using Catalog.Core.Result;
using Catalog.Core.Constants;
using Mapster;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace Catalog.Infrastructure.Mappings;

public static class MapConfig
    {
        public static TypeAdapterConfig AllProductsPagedConfig()
        {
            var config = new TypeAdapterConfig();
            config.NewConfig<JObject,  PaginatedResult<ProductDto>>()
                .MapWith(j => MapToAllProducts(j));
            return config;
        }

        public static TypeAdapterConfig SingleProductConfig()
        {
            var config = new TypeAdapterConfig();
            config.NewConfig<JObject, Result<DetailedProductDto>>()
                .MapWith(j => MapToSingleProduct(j));
            return config;
        }

        private static PaginatedResult<ProductDto> MapToAllProducts(JObject jsonObject)
        {
            try
            {
                List<ProductDto> products = new();
                JArray array = (JArray?)jsonObject[StrapiApiConstants.ApiResponse.DataRoot] ?? new JArray();
                foreach (var item in array)
                {
                    var product = MapToProduct((JObject)item);

                    if (product.Succeeded)
                        products.Add(product.Data!);
                }

                // ToDo: implement pagination
                int postCount = 2;// meta pagination total
                int page = 1;// meta pagination page
                int pageSize = 25; // meta pagination pageSize

                return PaginatedResult<ProductDto>.Success(products, postCount, page, pageSize);
                
            }
            catch (Exception ex)
            {
                // log error
                return PaginatedResult<ProductDto>.Failure("Failed to retrieve products");
            }
        }

        private static Result<DetailedProductDto> MapToSingleProduct(JObject jsonObject)
        {
            try
            {
                var dataObject = jsonObject[StrapiApiConstants.ApiResponse.DataRoot] ?? new JObject();
                var product = MapToProduct(dataObject).Data!;
                DetailedProductDto detailedProduct = new(product);
                detailedProduct.Body = dataObject["attributes"]?["body"]?.ToString() ?? string.Empty;
                detailedProduct.Price = (decimal)dataObject["attributes"]?["price"]!;
                detailedProduct.Qty = (int)dataObject["attributes"]?["qty"]!;
                detailedProduct.UpdatedAt = product.UpdatedAt;
                detailedProduct.Gallery = TryParseImageGalleryData(dataObject);
                return Result<DetailedProductDto>.Success(detailedProduct);
            }
            catch (Exception ex)
            {
                return Result<DetailedProductDto>.Failure("Failed to retrieve product details");
            }
        }
        
        private static Result<ProductDto> MapToProduct(JToken jsonObject)
        {
            var product = new ProductDto();
            try
            {
                product.Id = (int)jsonObject["id"]!;
                product.Name = jsonObject["attributes"]?["name"]?.ToString() ?? string.Empty;
                product.Description = jsonObject["attributes"]?["description"]?.ToString() ?? string.Empty;
                string dateTimeAsStr = jsonObject["attributes"]?["updatedAt"]?.ToString() ?? string.Empty;

                var res = TryParseDateTimeString(dateTimeAsStr);
                if (res.Succeeded)
                    product.UpdatedAt = res.ParsedDateTime;

                var imageJsonToken = jsonObject["attributes"]?["image"]?["data"];
                if (imageJsonToken is not null)
                {
                    var result = TryParseImageData(imageJsonToken);
                    if (result.Succeeded)
                        product.Image = result.Data;
                }

                return Result<ProductDto>.Success(product);
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Failure("Failed to retrieve product");
            }
        }
        
        

        private static (bool Succeeded, DateTime? ParsedDateTime) TryParseDateTimeString(string dtStr)
        {
            string[] formats = { "dd.MM.yyyy HH:mm:ss" };
            bool parseSucceeded = DateTime.TryParseExact(
                dtStr,
                formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime parsedDt);

            if (parseSucceeded)
                return (parseSucceeded, parsedDt);

            return (parseSucceeded, null);
        }

        private static Result<Image> TryParseImageData(JToken j, ImageFormat imageFormat = ImageFormat.Default)
        {
            try
            {
                if (!j.HasValues)
                    return Result<Image>.Failure("Failed to retrieve image");
                
                Image image = new Image();
                string widthStr = string.Empty;
                string heightStr = string.Empty;
                int width = 0;
                int height = 0;
                bool isValid = false;
                
                if (imageFormat != ImageFormat.Default)
                {
                    string format = imageFormat.ToString().ToLower();
                    image.Url =
                        j["attributes"]?["formats"]?
                            [format]?["url"]?.ToString() ?? string.Empty;
                    
                     widthStr = j["attributes"]?["formats"]?
                            [format]?["width"]?.ToString() ?? string.Empty;
                        
                    isValid = Int32.TryParse(widthStr, out width);
                    if (isValid)
                        image.Width = width;
                    
                    heightStr = j["attributes"]?["formats"]?
                            [format]?["height"]?.ToString() ?? string.Empty;
                    
                    isValid = Int32.TryParse(heightStr, out height);
                    if (isValid)
                        image.Height = height;
                    
                    return Result<Image>.Success(image);
                }

                    
                string? url = j["attributes"]?["url"]?.ToString() ?? string.Empty;
            
                if (!string.IsNullOrEmpty(url))
                    image.Url = url; 
                
                widthStr = j["attributes"]?["width"]?.ToString() ?? string.Empty;
                    
                isValid = Int32.TryParse(widthStr, out width);
                if (isValid)
                    image.Width = width;
                
                heightStr = j["attributes"]?["height"]?.ToString() ?? string.Empty;
                
                isValid = Int32.TryParse(heightStr, out height);
                if (isValid)
                    image.Height = height;
                return Result<Image>.Success(image);
                    
            }
            catch (Exception ex)
            {
                return Result<Image>.Failure("Failed to retrieve image");
            }
        }

        private static List<Image> TryParseImageGalleryData(JToken jsonObject)
        {
            var gallery = new List<Image>();
            try
            {
                JArray galleryArray = (JArray?)jsonObject["attributes"]?["gallery"]?["data"] ?? new JArray();
                foreach (var jsonToken in galleryArray)
                {
                    var imageParseResult = TryParseImageData(jsonToken);
                    if (imageParseResult.Succeeded)
                        gallery.Add(imageParseResult.Data!);
                }
                return gallery;
            }
            catch (Exception)
            {
                return gallery;
            }
        }
    }