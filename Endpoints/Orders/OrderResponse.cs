namespace iWantApp.Endpoints.Orders;

public record OrderResponse(Guid Id, string ClientEmail, IEnumerable<OrderProducts> Products,
    string DeliveryAddress);

public record OrderProducts(Guid id, string Name);