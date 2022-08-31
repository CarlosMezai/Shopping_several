using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }


        //Creo una propiedad que estabece el mombre, agrego requiered pra establecer los requisito de esta propiedad mediante los dataannotations
        [Display(Name ="País")]
        //especifica en erro al usuario en español, el 1 se reemplaza por los caracteres y el cero por el nombre del país
        [MaxLength(50, ErrorMessage ="El campo {0} debe tener máximo {1} caractéres.")]
        //El cero se reeemplaza por lo que tenga en la porpiedad Name
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public string Name { get; set; }
    }
}
