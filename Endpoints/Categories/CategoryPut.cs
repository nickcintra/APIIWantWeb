﻿using iWantApp.Domain.Products;
using iWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace iWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        category.EditInfo(categoryRequest.Name, categoryRequest.Active);


        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());


        context.SaveChanges();

        return Results.Ok();
    }
}
