﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind2.Data;

namespace Northwind2.Controllers
{
    public static class ControllerBaseExtensions
    {
        // Renvoie une réponse HTTP personnalisée pour les erreurs
        public static ActionResult CustomResponseForError(this ControllerBase controller, Exception ex)
        {
            if (ex is DbUpdateException e)
            {
                ProblemDetails pb = e.ConvertToProblemDetails();
                return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
            }
            else throw ex;
        }
    }
}
