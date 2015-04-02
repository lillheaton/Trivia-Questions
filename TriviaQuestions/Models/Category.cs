using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaQuestions.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; } 
    }
}