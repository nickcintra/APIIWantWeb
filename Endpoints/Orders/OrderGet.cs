using System.Security.Claims;

namespace iWantApp.Endpoints.Orders;
public class OrderGet
{
    public static string Template => "/orders/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static async Task<IResult> Action(
        Guid id, HttpContext http, ApplicationDbContext context,UserManager<IdentityUser> userManager)
    {
        var clientClaim = http.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier);
        var employeeClaim = http.User.Claims
            .FirstOrDefault(c => c.Type == "EmployeeCode");

        var order = context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == id);

        if (order.ClientId != clientClaim.Value && employeeClaim == null)
            return Results.Forbid();

        var client = await userManager.FindByIdAsync(order.ClientId);

        var productsReponse = order.Products.Select(p => new OrderProducts(p.Id, p.Name));
        var orderResponse = new OrderResponse(
            order.Id, client.Email, productsReponse, order.DeliveryAddress);

        return Results.Ok(orderResponse);
    }
}
