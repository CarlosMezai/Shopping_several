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
        //El cero se reeemplaza por lo que tenga en la propiedad Name
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public string Name { get; set; }

        //ahora se realiza la relación de muchos a uno
        public  ICollection<State> States { get; set; }




        //Crear una propiedad entera que permita conocer la cantidad de estados que tiene un pais
        //de igual forma hay que establecer una condición que consulte si es nula ponga 0 y sino porga la cantidad de estados
        [Display(Name = "Departamentos/Estados")]
        //Aplicamos un operador ternario, una condicon if si los states es = null me regrsa un 0 de lo contrario
        //el conteo de estados asosciado al país
        public int StatesNumber => States == null ? 0 : States.Count;
    }
}
