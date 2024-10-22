using System.ComponentModel.DataAnnotations;
using AngularApp4.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularApp4.Server.Validation
{
    public class ValidarClienteRut:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            var cliente = validationContext.ObjectInstance as Cliente;

            var entity = context!.Cliente.AsNoTracking()
                                        .FirstOrDefault(c => c.Rut == (string)value && c.Id != cliente.Id);

            if (entity != null)
            {
                return new ValidationResult("El Rut ya existe.");
            }

            return ValidationResult.Success;
        }
    }
}
