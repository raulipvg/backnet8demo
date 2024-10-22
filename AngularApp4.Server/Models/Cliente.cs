using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AngularApp4.Server.Validation;

namespace AngularApp4.Server.Models
{
    public class Cliente
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 9)]
        [RegularExpression("^[0-9-Kk]*$", ErrorMessage = "Rut No valido")]
        [ValidarClienteRut]
        public string Rut { get; set; }


        [Required]
        [StringLength(50, MinimumLength =3)]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Solo se permiten letras")]
        public string Nombre { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Solo se permiten letras")]
        public string Apellido { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        
        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Display(Name = "Pais")]
        public int PaisId { get; set; }
        public virtual Pais? Pais { get; set; }

     
    }
}
