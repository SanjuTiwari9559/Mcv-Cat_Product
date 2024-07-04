using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task_Cat_ProMvc.Models.viewModel
{
    public class EditProductRequest
    {
        public int Id { get; set; }

      
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
    }
}
