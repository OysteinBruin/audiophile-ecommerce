using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Catalog.Tests.Integration.Mocks;
using WireMock.Server;

public class MockedStrapiApiServer : IDisposable
{
    private WireMockServer _server;
    
    public string Url => _server.Url!;

    public void Start()
    {
        _server = WireMockServer.Start();
        _server.Given(Request.Create()
            .WithPath("/api/products*")
            .UsingGet())
            .RespondWith(Response.Create()
                .WithBody(@"{
  ""data"": [
        {
            ""id"": 1,
            ""attributes"": {
                ""name"": ""ZX9 SPEAKER"",
                ""description"": ""Upgrade your sound system with the all new ZX9 active speaker. It’s a bookshelf speaker system that offers truly wireless connectivity -- creating new possibilities for more pleasing and practical audio setups."",
                ""image"": {
                    ""data"": {
                        ""id"": 5,
                        ""attributes"": {
                            ""name"": ""zx9-product.jpg"",
                            ""alternativeText"": ""ZX9 SPEAKER"",
                            ""caption"": null,
                            ""width"": 1080,
                            ""height"": 1120,
                            ""formats"": {
                                ""thumbnail"": {
                                    ""name"": ""thumbnail_image-product.jpg"",
                                    ""hash"": ""thumbnail_image_product_6bcd712d2a"",
                                    ""ext"": "".jpg"",
                                    ""mime"": ""image/jpeg"",
                                    ""path"": null,
                                    ""width"": 150,
                                    ""height"": 156,
                                    ""size"": 2.36,
                                    ""url"": ""/uploads/thumbnail_image_product_6bcd712d2a.jpg""
                                },
                                ""large"": {
                                    ""name"": ""large_image-product.jpg"",
                                    ""hash"": ""large_image_product_6bcd712d2a"",
                                    ""ext"": "".jpg"",
                                    ""mime"": ""image/jpeg"",
                                    ""path"": null,
                                    ""width"": 964,
                                    ""height"": 1000,
                                    ""size"": 29.86,
                                    ""url"": ""/uploads/large_image_product_6bcd712d2a.jpg""
                                },
                                ""small"": {
                                    ""name"": ""small_image-product.jpg"",
                                    ""hash"": ""small_image_product_6bcd712d2a"",
                                    ""ext"": "".jpg"",
                                    ""mime"": ""image/jpeg"",
                                    ""path"": null,
                                    ""width"": 482,
                                    ""height"": 500,
                                    ""size"": 11.01,
                                    ""url"": ""/uploads/small_image_product_6bcd712d2a.jpg""
                                },
                                ""medium"": {
                                    ""name"": ""medium_image-product.jpg"",
                                    ""hash"": ""medium_image_product_6bcd712d2a"",
                                    ""ext"": "".jpg"",
                                    ""mime"": ""image/jpeg"",
                                    ""path"": null,
                                    ""width"": 723,
                                    ""height"": 750,
                                    ""size"": 19.62,
                                    ""url"": ""/uploads/medium_image_product_6bcd712d2a.jpg""
                                }
                            },
                            ""hash"": ""image_product_6bcd712d2a"",
                            ""ext"": "".jpg"",
                            ""mime"": ""image/jpeg"",
                            ""size"": 26.51,
                            ""url"": ""/uploads/image_product_6bcd712d2a.jpg"",
                            ""previewUrl"": null,
                            ""provider"": ""local"",
                            ""provider_metadata"": null,
                            ""createdAt"": ""2023-08-19T06:59:30.524Z"",
                            ""updatedAt"": ""2023-08-19T07:01:35.588Z""
                        }
                    }
                }
            }
        },
        {
            ""id"": 2,
            ""attributes"": {
                ""name"": ""ZX7 SPEAKER"",
                ""description"": ""Stream high quality sound wirelessly with minimal loss. The ZX7 bookshelf speaker uses high-end audiophile components that represents the top of the line powered speakers for home or studio use."",
                ""image"": {
                    ""data"": null
                }
            }
        }
    ],
    ""meta"": {
        ""pagination"": {
            ""page"": 1,
            ""pageSize"": 25,
            ""pageCount"": 1,
            ""total"": 2
        }
    }
}")

                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));
    }
    
    public void Dispose()
    {
        _server.Stop();
        _server.Dispose();
    }
}