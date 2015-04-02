using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaQuestion2.Models
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
    }
}